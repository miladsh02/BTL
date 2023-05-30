using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Product()
        {
            return View();
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
