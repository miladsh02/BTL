using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DtoModels
{
    public class AddProductDto
    {
        public Guid ProductTemplateId { get; set; }
        public int ProductQuantity { get; set; }
        public float ProductPrice { get; set; }
        public DateTime ProductOrderDate { get; set; }


    }
}
