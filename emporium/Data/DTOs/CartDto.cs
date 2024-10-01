using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class CartDto
    {
        public Guid userId { get; set; }
        public Guid cartId { get; set; }
        public double total_before_discount { get; set; }
        public double total_after_discount { get; set; }
        public double shipping_cost { get; set; }
        public double final_total {  get; set; }
    }
}
