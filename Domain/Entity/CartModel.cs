using System.ComponentModel.DataAnnotations;
using BTL.Enums;

namespace BTL.Models
{
    public class CartModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid StudentId { get; set; }
        public CartStatus Status { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set;}
        public DateTime RemovedDate { get; set;}
    }
}
