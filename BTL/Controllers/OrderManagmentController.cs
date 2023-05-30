using BTL.Data;
using BTL.Enums;
using Domain.DtoModels;
using Domain.Enums;
using Domain.Models.Composite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    public class OrderManagmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderManagmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Orders()
        {
            DateTime yesterdayDateTime = DateTime.Now.AddDays(-1);

            var results = await(from o in _context.Order
                join p in _context.Products on o.ProductId equals p.Id
                join pt in _context.ProductsTemplate on p.ProductTemplateId equals pt.Id
                join s in _context.Students on o.StudentId equals s.Id
                where (o.DeliveryDate >= DateTime.Today) &&
                      (o.DeliveryDate <= DateTime.Today.AddDays(1)) &&
                      (o.Status == OrderStatus.InProcess)
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
                    StudentPhoneNumber = s.PhoneNumber,
                    StudentUniversityId = s.UniversityId
                }).ToListAsync();


            return View(results);
        }

        public IActionResult EditOrdersToDelivered(Guid oderId)
        {
            return View();
        }

    }
}
