﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities
{
    public class Review
    {
        [Key]
        public Guid Review_id { get; set; }

        [MaxLength(2000)]
        public string? Review_content { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Review_rate { get; set;}

        [ForeignKey("Product")]
        public Guid? Product_id { get; set; }
        public Product? Product { get; set; }

        [ForeignKey("User")]
        public Guid? User_id { get; set; }
        public User? User { get; set; }   
    }
}
