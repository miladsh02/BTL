﻿using System.ComponentModel.DataAnnotations;

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
    }
}