using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class CartDto
    {
        public required Guid userId { get; set; }
        public required Guid cartId { get; set; }
        public List<ProductDto>? products { get; set; }
        public double? totalBeforeDiscount { get; set; }
        public double? totalAfterDiscount { get; set; }
        public double shippingCost { get; set; }
        public double finalTotal {  get; set; }
    }
}
