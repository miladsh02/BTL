using BTL.Data;
using BTL.Models;
using Data.Entity;
using Domain.DtoModels;
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
        
        public async Task<IActionResult> AddProduct(AddProductDto product)
        {
            await _context.Products.AddAsync(new ProductModel()
            {
                Id = new Guid(),
                ProductTemplateId = product.ProductTemplateId,
                Quantity = product.ProductQuantity,
                Price = product.ProductPrice,
                OrderDate = product.ProductOrderDate
            });

            return RedirectToAction(nameof(ProductController.AddProductPanel));
        }
        
        public async Task<IActionResult> Products()
        {
            var results = await (from p in _context.Products
                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                where p.OrderDate >= DateTime.Today && p.OrderDate <= DateTime.Today.AddDays(1) && p.Quantity > 0
                select new ManageProductDto()
                {
                    ProductId = p.Id,
                    ProductTemplateId= pt.Id,
                    ProductTemplateName = pt.Name,
                    ProductTemplateDescription = pt.Description,
                    ProductPrice = p.Price,
                    ProductQuantity = p.Quantity,
                    ProductOrderDate = p.OrderDate
                }).ToListAsync();

            return View(results);
        }
        
        public IActionResult EditProductPanel(ManageProductDto product)
        {
            return View(product);
        }
        
        public async Task<IActionResult> EditProduct(ManageProductDto product)
        {
            var updatedObj = new ProductModel()
            {
                Id = product.ProductId,
                ProductTemplateId = product.ProductTemplateId,
                OrderDate = product.ProductOrderDate,
                Price = product.ProductPrice,
                Quantity = product.ProductQuantity
            };
            
            _context.Products.Update(updatedObj);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(ProductController.EditProductPanel), product);
        }
        
        public async Task<IActionResult> ProductTemplate()
        {
            var obj = await _context.ProductsTemplate.ToListAsync();
            return View(obj);
        }
        
        public IActionResult AddProductTemplatePanel()
        {
            return View();
        }        
        
        public async Task<IActionResult> AddProductTemplate(ProductTemplateModel productTemplate)
        {
            await _context.AddAsync(productTemplate);
            await _context.SaveChangesAsync();
            return View();
        }
        
        public IActionResult EditProductTemplatePanel(ProductTemplateModel productTemplate)
        {
            return View(productTemplate);
        }
        
        public async Task<IActionResult> EditProductTemplate(ProductTemplateModel productTemplate)
        {
            _context.ProductsTemplate.Update(productTemplate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProductController.EditProductTemplatePanel), productTemplate);
        }
    }
}
