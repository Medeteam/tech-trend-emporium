using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Cart
    {
        [Key]
        public Guid Cart_id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("User")]
        public Guid User_id { get; set; }
        public User User { get; set; }

        [ForeignKey("Coupon")]
        public Guid Coupon_id { get; set; }
        public Coupon Coupon { get; set; }

        [ForeignKey("ShoppingStatus")]
        public Guid Shopping_status_id { get; set; }
        public ShoppingStatus ShoppingStatus { get; set; }

        public Order Order { get; set; }

        public List<ProductToCart> ProductToCarts { get; set; } = new List<ProductToCart>();
    }
}
