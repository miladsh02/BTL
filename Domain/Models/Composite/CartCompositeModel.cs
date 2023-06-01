using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL.Models;
using Data.Entity;

namespace Domain.Models.Composite
{
    public class CartCompositeModel
    {
        public CartModel? Cart { get; set; }
        public ProductModel? Product { get; set; }
        public ProductTemplateModel? ProductTemplate{ get; set; }


    }
}
