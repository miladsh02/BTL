using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL.Models;
using Domain.Enums;
using Domain.Entity;

namespace Data.Entity
{
    public class OrderModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RemovedDate { get; set; }
        public Guid StudentId { get; set; } 
        public Guid ProductId { get; set; }
        public ProductModel? Product { get; set; }




    }
}
