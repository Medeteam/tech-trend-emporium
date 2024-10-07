using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ReviewDto
    {
        public string user { get; set; }
        public Guid productId { get; set; }
        public decimal rate { get; set; }
        public string comment { get; set; }
    }
}
