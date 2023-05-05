using System.ComponentModel.DataAnnotations;
using Data.Entity;

namespace BTL.Models
{
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public float Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<OrderModel>? Order { get; set; }
        public ICollection<CartModel> Carts { get; } = new List<CartModel>();

    }
}
