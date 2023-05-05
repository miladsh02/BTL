﻿using Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BTL.Models
{
    public class StudentModel
    {
        [Key]
        public Guid Id { get; set; }
        public string UniversityId { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserId{ get; set; } = null!;
        public CustomIdentityUserModel CustomIdentityUserModel { get; set; }

        public ICollection<OrderModel> Orders { get; } = new List<OrderModel>(); 
        public ICollection<CartModel> Carts { get; } = new List<CartModel>(); 




        public DateTime CreatedDate { get; set; }


    }
}
