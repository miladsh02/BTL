using BTL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddProductPanel()
        {
            var productTemplate = await _context.ProductsTemplate.ToListAsync();
            return View(productTemplate);
        }

        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        public async Task<IActionResult> EditProduct()
        {
            return View();
        }
        
        public async Task<IActionResult> ProductTemplate()
        {
            return View();
        }

        public async Task<IActionResult> AddProductTemplate()
        {
            return View();
        }

        public async Task<IActionResult> EditProductTemplate()
        {
            return View();
        }
    }
}
