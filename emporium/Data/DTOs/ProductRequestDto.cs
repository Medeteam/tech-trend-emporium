using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ProductRequestDto
    {
        public Guid productId { get; set; }
        public int quantity { get; set; }
    }

}
