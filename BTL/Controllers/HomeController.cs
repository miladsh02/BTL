﻿using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BTL.Data;
using BTL.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Cryptography.Xml;
using Data.Entity;
using Domain.DtoModels;
using Domain.Entity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LoginModel = BTL.Areas.Identity.Pages.Account.LoginModel;
using Domain.Models.Composite;
using System.Security.Principal;
using NuGet.Packaging;
using Microsoft.CodeAnalysis;

namespace BTL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string?  _loginUrl;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _loginUrl = "Identity/Account/Login";
        }
        
        
        //[Authorize(Roles = "Admin,developer")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Products()
        {
            var results = await (from p in _context.Products
                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                where p.OrderDate >= DateTime.Today && p.OrderDate <= DateTime.Today.AddDays(1)&&p.Quantity>0
                select new ProductDto
                {
                    ProductId = p.Id,
                    ProductTemplateName = pt.Name,
                    ProductTemplateDescription = pt.Description,
                    ProductPrice = p.Price,
                    ProductQuantity = p.Quantity,
                    ProductOrderDate = p.OrderDate
                }).ToListAsync();

            return View(results);
        }
        
        public async Task<IActionResult> AddToCart(List<ProductDto> products)
        {
            #region Get User Data

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return Redirect(_loginUrl!);
            }

            var student = await _context.Students.FirstOrDefaultAsync(s=>s.UserId==userId);
            if (student!.Id.ToString().IsNullOrEmpty())
            {
                var loginUrl = Url.RouteUrl(new { area = "Identity", page = "/Account/Login" });
                return Redirect(loginUrl!);
            }

            #endregion

            #region UserCartData

            List<CartCompositeModel> compositeData =
                await (from s in _context.Students
                    join c in _context.Carts on s.Id equals c.StudentId
                    join p in _context.Products on c.ProductId equals p.Id
                    join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                    where (c.Status == CartStatus.Available) || (c.Status == CartStatus.ChangedTheValue) || (c.Status == CartStatus.Unavailable)
                        && s.UserId == userId
                       select new CartCompositeModel()
                    {
                        Cart = c,
                        Product = p,
                        ProductTemplate = pt
                    }).ToListAsync();
            var customerCart = new List<CartModel>();
            customerCart.AddRange(compositeData.Select(model => model.Cart));
            #endregion

            #region Add products to cart

            var cartList = new List<CartModel>();

            foreach (var product in products)
            {
                if (customerCart.Exists(model => model.ProductId == product.ProductId))
                {
                    var updatedCart = customerCart.Find(model => model.ProductId == product.ProductId);
                    updatedCart.Quantity += 1;
                    _context.Carts.Update(updatedCart);
                }
                else
                {
                    
                    cartList.Add(new CartModel()
                    {
                        CreatedDate = DateTime.Now,
                        Id = new Guid(),
                        ProductId = product.ProductId,
                        Quantity = 1,
                        StudentId = student.Id,
                        Status = CartStatus.Available
                    });
                }
                
            }

            #endregion
            
            await _context.Carts.AddRangeAsync(cartList);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HomeController.Cart));
        }
        public async Task<IActionResult> Cart()
        {
            #region Validate User

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return Redirect(_loginUrl!);
            }

            #endregion

            #region get user cart data from DB


            DateTime yesterdayDateTime = DateTime.Now.AddDays(-1);

            List<CartCompositeModel> results =
                await (from s in _context.Students
                       join c in _context.Carts on s.Id equals c.StudentId
                       join p in _context.Products on c.ProductId equals p.Id
                       join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                       where (c.Status == CartStatus.Available) || (c.Status == CartStatus.ChangedTheValue) 
                              || (c.Status == CartStatus.Unavailable) && (s.UserId == userId )
                       select new CartCompositeModel()
                       {
                           Cart = c,
                           Product = p,
                           ProductTemplate = pt
                       }).ToListAsync();

            #endregion

            #region update the cart data

            foreach (var result in results)
            {


                #region update status
                //change Unavailable to RemovedFromCart
                if (result.Cart.Status == CartStatus.Unavailable)
                {
                    result.Cart.Status = CartStatus.RemovedFromCart;
                }

                //check date
                if (result.Product.OrderDate < yesterdayDateTime)
                {
                    result.Cart.Status = CartStatus.RemovedFromCart;
                }

                //change ChangedTheValue to Available
                else if (result.Cart.Status == CartStatus.ChangedTheValue)
                {
                    result.Cart.Status = CartStatus.Available;
                }
                #endregion 

                //find object
                var dbSearch =
                    await _context.Products.FirstOrDefaultAsync(x => x.Id == result.Product.Id);

                //change status to Unavailable
                if (dbSearch == null)
                {
                    result.Cart.Status = CartStatus.Unavailable;
                }

                //update value
                else if (result.Cart.Quantity > dbSearch.Quantity)
                {
                    result.Cart.Quantity = dbSearch.Quantity;
                    result.Cart.Status = CartStatus.ChangedTheValue;
                }

            }
            _context.UpdateRange(results.Select(model => model.Cart));
            await _context.SaveChangesAsync();

            #endregion

            #region mapping data

            //map data to dto model
            var dtoObject = new List<CartDtoModel>();
            foreach (var result in results)
            {
                dtoObject.Add(new CartDtoModel()
                {
                    CartId = result.Cart.Id,
                    ProductName = result.ProductTemplate.Name,
                    ProductPrice = result.Product.Price,
                    Quantity = result.Cart.Quantity,
                    Status = result.Cart.Status

                });
            }

            #endregion

            return View(dtoObject);
        }

        public async Task<IActionResult> Transaction()
        {
            #region check user
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return Redirect(_loginUrl!);
            }

            var studentId = await 
                (
                 from s in _context.Students
                 where s.UserId == userId
                 select s.Id
                 ).FirstOrDefaultAsync();
            #endregion

            #region GetUsersCartData
            List<CartCompositeModel> results =
                await (from s in _context.Students
                    join c in _context.Carts on s.Id equals c.StudentId
                    join p in _context.Products on c.ProductId equals p.Id
                    join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                    where (c.Status == CartStatus.Available) && s.UserId==userId
                    select new CartCompositeModel()
                    {
                        Cart = c,
                        Product = p,
                        ProductTemplate = pt
                    }).ToListAsync();
            
            if (results.IsNullOrEmpty())
                return RedirectToAction(nameof(HomeController.Cart));
            #endregion

            #region Transport Cart Data to order and make a transaction
            var price = new float();
            foreach (var cartItem in results)
            {
                var product =
                    await _context.Products.FirstOrDefaultAsync(x => x.Id == cartItem.Product.Id);

                cartItem.Cart.Status = CartStatus.AddedToOrder;
                var productQuantity= product.Quantity;
                var cartQuantity= cartItem.Cart.Quantity;
                product.Quantity = productQuantity - cartQuantity;
                _context.Products.Update(product);
                _context.Carts.Update(cartItem.Cart);
                price += cartItem.Product.Price * cartItem.Cart.Quantity;

                await _context.Order.AddAsync(new OrderModel()
                {
                    Id = new Guid(),
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Cart.Quantity,
                    DeliveryDate = cartItem.Product.OrderDate,
                    StudentId = studentId,
                    Status = OrderStatus.InTransaction
                });
            }


            #endregion
            
            await _context.SaveChangesAsync();
            return View(price);
        }

        public async Task<IActionResult> AddToOrder(bool transactionResult)
        {
            #region Get user data

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return Redirect(_loginUrl!);
            }

            var studentId = await
            (
                from s in _context.Students
                where s.UserId == userId
                select s.Id
            ).FirstOrDefaultAsync();

            #endregion
            
            
            if (transactionResult is true)
            {
                var resultList = await _context.Order
                    .Where(o => o.StudentId == studentId &&o.Status==OrderStatus.InTransaction)
                    .ToListAsync();

                foreach (var result in resultList)
                {
                    result.Status = OrderStatus.InProcess;
                }
                _context.UpdateRange(resultList);
            }

            if (transactionResult is false)
            {
                var resultList = await _context.Order
                    .Where(o => o.StudentId == studentId && o.Status == OrderStatus.InTransaction)
                    .ToListAsync();
                foreach (var result in resultList)
                {
                    result.Status = OrderStatus.TransactionFailed;
                    
                }

                foreach (var result in resultList)
                {
                    var updatedProduct =
                        await _context.Products.FirstOrDefaultAsync(x=>x.Id==result.ProductId);
                    updatedProduct.Quantity = updatedProduct.Quantity+result.Quantity;
                    _context.Update(updatedProduct);
                }

                _context.UpdateRange(resultList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(HomeController.MyOrders));
        }

        public async Task<IActionResult> MyOrders()
        {
            #region Get user data

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return Redirect(_loginUrl!);
            }

            var studentId = await
            (
                from s in _context.Students
                where s.UserId == userId
                select s.Id
            ).FirstOrDefaultAsync();

            #endregion

            var results = await (from o in _context.Order
                join p in _context.Products on o.ProductId equals p.Id
                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                join s in _context.Students on o.StudentId equals s.Id
                where (o.DeliveryDate >= DateTime.Today) &&
                      (o.DeliveryDate <= DateTime.Today.AddDays(1)) &&
                      (o.Status == OrderStatus.InProcess) &&
                      (s.UserId == userId)
                                 select new OrderDtoModel()
                                 {
                                     OrderId = o.Id,
                                     ProductName = pt.Name,
                                     OrderQuantity = o.Quantity,
                                     Price = p.Price,
                                     StudentUniversityId = s.UniversityId,
                                     StudentLastName = s.LastName,
                                     StudentFirstName = s.FirstName,
                                     StudentPhoneNumber = s.PhoneNumber,
                                     OrderDeliveryDate = o.DeliveryDate,
                                     OrderStatus = o.Status
                                 }).ToListAsync();

            return View(results);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CustomError()
        {
            return View();
        }
    }

}