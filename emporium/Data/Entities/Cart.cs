using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Cart
    {
        [Key]
        public Guid Cart_id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("Coupon")]
        public Guid? Coupon_id { get; set; }
        public Coupon? Coupon { get; set; }
        public string? Coupon_name { get; set; }

        [ForeignKey("ShoppingStatus")]
        public Guid? Shopping_status_id { get; set; }
        public ShoppingStatus? ShoppingStatus { get; set; }

        [ForeignKey("Order")]
        public Guid? Order { get; set; }
        public Order? Orders { get; set; }
        public List<ProductToCart>? ProductToCart { get; set; }

    }
}
