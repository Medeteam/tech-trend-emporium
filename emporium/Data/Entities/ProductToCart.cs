﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ProductToCart
    {
        [Key]
        public Guid Product_cart_id { get; set; } = Guid.NewGuid();

        public int Quantity { get; set; }

        [ForeignKey("Cart")]
        public Guid Cart_id { get; set; }
        public Cart Cart { get; set; }


        [ForeignKey("Product")]
        public Guid Product_id { get; set; }
        public Product Product { get; set; }
    }
}