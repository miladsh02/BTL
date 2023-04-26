using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTL.Models;

namespace Data.Entity
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public ICollection<StudentOrderModel>? students { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
