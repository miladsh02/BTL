using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class OrderManagmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult AddProductTemplate()
        {
            return View();
        }

    }
}
