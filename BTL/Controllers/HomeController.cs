using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BTL.Data;
using BTL.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Domain.DtoModels;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LoginModel = BTL.Areas.Identity.Pages.Account.LoginModel;

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
        public async Task<IActionResult> AddToCart(List<ProductModel> products)
        {
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

            foreach (var product in products)
            {
                var cart = new CartModel()
                {
                    CreatedDate = DateTime.Now,
                    Id = new Guid(),
                    ProductId = product.Id,
                    Quantity = 1,
                    StudentId = product.Id,
                    Status = CartStatus.Available
                };
                await _context.Carts.AddAsync(cart);
                
            }

            await _context.SaveChangesAsync();
            return Redirect("~/Index");
        }

        public async Task<IActionResult> Cart()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return Redirect(_loginUrl!);
            }

            List<CartDtoModel> results =
                await (from s in _context.Students
                    join c in _context.Carts on s.Id equals c.StudentId
                    join p in _context.Products on c.ProductId equals p.Id
                    join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                    where (c.Status == CartStatus.Available) || (c.Status == CartStatus.Unavailable)
                    select new CartDtoModel()
                    {
                        CartId = c.Id,
                        ProductName = pt.Name,
                        Quantity = c.Quantity,
                        ProductPrice = p.Price,
                        Status = c.Status
                    }).ToListAsync();


            //if()

            return View();
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