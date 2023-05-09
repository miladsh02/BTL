using BTL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DtoModels
{
    public class CartDtoModel
    {
        public Guid CartId { get; set; }
        public CartStatus Status { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public float ProductPrice { get; set; }
    }
}
