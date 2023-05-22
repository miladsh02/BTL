using BTL.Models;
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
            _loginUrl = Url.RouteUrl(new { area = "Identity", page = "/Account/Login" }).IsNullOrEmpty()?
                        Url.RouteUrl(new { area = "Identity", page = "/Account/Login" }):
                        nameof(HomeController.CustomError);
        }
        
        
        //[Authorize(Roles = "Admin,developer")]
        public async Task<IActionResult> Index()
        {
            var results =await (from p in _context.Products
                                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                                where p.OrderDate >= DateTime.Today&& p.OrderDate <= DateTime.Today.AddDays(1)
                                select new ProductDto 
                                {
                                 ProductId = p.Id,ProductTemplateName = pt.Name,
                                 ProductTemplateDescription = pt.Description,ProductPrice = p.Price,
                                 ProductQuantity = p.Quantity,ProductOrderDate = p.OrderDate
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

            #region Add products to cart

            var cartList = new List<CartModel>();

            foreach (var product in products)
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

            #endregion
            
            await _context.Carts.AddRangeAsync(cartList);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HomeController.Cart));
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

                await _context.Order.AddAsync(new OrderModel()
                {
                    CreatedDate = DateTime.Now,
                    DeliveryDate = product.OrderDate,
                    Id = new Guid(),
                    ProductId = product.Id,
                    Quantity = cartItem.Cart.Quantity,
                    Status = OrderStatus.InTransaction,
                    StudentId = studentId
                });

                var productQuantity= product.Quantity;
                var cartQuantity= cartItem.Product.Quantity;
                product.Quantity = productQuantity - cartQuantity;
                _context.Update(product);
                price += cartItem.Product.Price * cartItem.Cart.Quantity;
            }

            await _context.Transaction.AddAsync(new TransactionModel()
            {
                CreateDate = DateTime.Now,
                Id = new Guid(),
                Status = StudentTransactionStatus.WhileTransaction,
                StudentId = studentId,
                Price = price
            });
            #endregion
            
            await _context.SaveChangesAsync();
            return View();
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
                    .Where(o => o.StudentId == studentId && o.Status == OrderStatus.InTransaction)
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
                    updatedProduct!.Quantity += result.Quantity;
                    _context.Update(updatedProduct);
                }

                _context.UpdateRange(resultList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction();
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
            
            List<CartCompositeModel> results =
                await (from s in _context.Students
                    join c in _context.Carts on s.Id equals c.StudentId
                    join p in _context.Products on c.ProductId equals p.Id
                    join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                    where (c.Status == CartStatus.Available) || (c.Status == CartStatus.Unavailable)
                    select new CartCompositeModel()
                    {
                        Cart = c,
                        Product = p,
                        ProductTemplate=pt
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
                    _context.Update(result);
                }
                //change ChangedTheValue to Available
                else if (result.Cart.Status == CartStatus.ChangedTheValue)
                {
                    result.Cart.Status = CartStatus.Available;
                    _context.Update(result);
                }
                #endregion 

                //find object
                var dbSearch =
                    await _context.Products.FirstOrDefaultAsync(x => x.Id == result.Product.Id);

                //change status to Unavailable
                if (dbSearch == null)
                {
                    result.Cart.Status = CartStatus.Unavailable;
                    _context.Update(result);
                }

                //update value
                else if (result.Cart.Quantity > dbSearch.Quantity)
                {
                    result.Cart.Quantity = dbSearch.Quantity;
                    result.Cart.Status = CartStatus.ChangedTheValue;
                }

            }
            
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

        public async Task<IActionResult> Orders()
        {
            var results = await (from o in _context.Order
                join p in _context.Products on o.ProductId equals p.Id
                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                join s in _context.Students on o.StudentId equals s.Id
                where (o.DeliveryDate >= DateTime.Today )&&
                      (o.DeliveryDate <= DateTime.Today.AddDays(1))&&
                      (o.Status==OrderStatus.InProcess)
                select new OrderDtoModel()
                {
                    OrderId = o.Id,
                    Price = p.Price,
                    OrderDeliveryDate = o.DeliveryDate,
                    ProductName = pt.Name,
                    OrderQuantity = o.Quantity,
                    OrderStatus = o.Status,
                    StudentFirstName = s.FirstName,
                    StudentLastName = s.LastName,
                    StudentPhoneNumber =s.PhoneNumber,
                    StudentUniversityId = s.UniversityId
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