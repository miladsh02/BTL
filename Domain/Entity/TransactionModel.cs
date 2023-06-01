using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BTL.Models;
using Data.Entity;
using Domain.Enums;

namespace Domain.Entity
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public CartModel? Cart { get; set; }
        public Guid StudentId { get; set; }
        public StudentModel? Student { get; set; }

        public float Price { get; set; }
        public StudentTransactionStatus Status { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
