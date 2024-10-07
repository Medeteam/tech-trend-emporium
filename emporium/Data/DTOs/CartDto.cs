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
        public List<ProductCartDto>? products { get; set; }
        public CartCouponDto coupon { get; set; }
        public decimal? totalBeforeDiscount { get; set; }
        public decimal? totalAfterDiscount { get; set; }
        public decimal shippingCost { get; set; }
        public decimal finalTotal {  get; set; }
    }
}
