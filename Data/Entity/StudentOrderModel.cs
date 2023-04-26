using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL.Models;

namespace Data.Entity
{
    public class StudentOrderModel
    {
        public Guid CustomerId { get; set; }
        public StudentModel Customer { get; set; }
        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
