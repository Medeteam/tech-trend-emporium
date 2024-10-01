using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ProductCartDto
    {
        public required Guid id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
        public int quantity { get; set; }
    }
}
