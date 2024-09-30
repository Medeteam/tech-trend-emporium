using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Coupon
    {
        
        [Key]
        public Guid Coupon_id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public required string Coupon_name { get; set; }

        public int Discount { get; set; }

        [MaxLength(255)]
        public required string Code { get; set; }

        public bool Coupon_status { get; set; }

        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        public List<Cart>? Carts { get; set; }
    }
}
