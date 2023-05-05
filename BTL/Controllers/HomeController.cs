using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BTL.Data;
using BTL.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Domain.DtoModels;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize(Roles = "Admin,developer")]
        public async Task<IActionResult> Index()
        {
            //var products =  _context.Products.Where
            //                    (x => x.OrderDate == DateTime.Today)
            //                    .ToListAsync();

            var results=await from p in _context.Products
                                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                                where p.OrderDate >= DateTime.Today&& p.OrderDate <= DateTime.Today.AddDays(1)
                                select new ProductDto 
                                {
                                 ProductId = p.Id,ProductTemplateName = pt.Name,
                                 ProductTemplateDescription = pt.Description,ProductPrice = p.Price,
                                 ProductQuantity = p.Quantity,ProductOrderDate = p.OrderDate
                                };
            return View();
        }
        public async Task<IActionResult> AddToCart(ProductModel product)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await _context.Students.FirstOrDefaultAsync(s=>s.UserId==userId);
            var cart = new CartModel()
            {
                CreatedDate = DateTime.Now,
                Id = new Guid(),
                ProductId = product.Id,
                Quantity = 1,
                StudentId = student.Id,
                status = CartStatus.Available
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return Redirect("~/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}