using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.DtoModels
{
    public class OrderDtoModel
    {
        public Guid OrderId { get; set; }
        public string ProductName { get; set; } = null!;
        public int OrderQuantity { get; set; }
        public float Price { get; set; }
        public string StudentUniversityId { get; set; } = null!;
        public string? StudentFirstName { get; set; }
        public string? StudentLastName { get; set; }
        public string? StudentPhoneNumber { get; set; }
        public OrderStatus OrderStatus{ get; set; }
        public DateTime OrderDeliveryDate{ get; set; }

    }
}
