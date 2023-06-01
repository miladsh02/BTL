using System.ComponentModel.DataAnnotations;
using Data.Entity;

namespace BTL.Models
{
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid ProductTemplateId { get; set; }
        public ProductTemplateModel? ProductTemplate { get; set; }

        public ICollection<OrderModel>? Order { get; set; }
        public ICollection<CartModel> Carts { get; } = new List<CartModel>();

    }
}
