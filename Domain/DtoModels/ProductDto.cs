using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DtoModels
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductTemplateName { get; set; } = null!;
        public string ProductTemplateDescription { get; set; } = null!;
        public float ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime ProductOrderDate { get; set; }

    }
}
