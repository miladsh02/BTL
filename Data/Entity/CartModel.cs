using System.ComponentModel.DataAnnotations;

namespace BTL.Models
{
    public class CartModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid StudentId { get; set; }
        public string status { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set;}

    }
}
