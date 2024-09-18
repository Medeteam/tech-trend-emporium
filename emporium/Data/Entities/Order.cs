using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Order
    {
        [Key]
        public Guid Order_id { get; set; } = Guid.NewGuid();
        public int Total_price { get; set; }

        [MaxLength(255)]
        public required string Address { get; set; }

        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("User")]
        public Guid User_id { get; set; }
        public User User { get; set; }

        [ForeignKey("ShoppingStatus")]
        public Guid Shopping_status_id { get; set; }
        public ShoppingStatus ShoppingStatus { get; set; }

        [ForeignKey("Cart")]
        public Guid Cart_id { get; set; }  // Clave foránea hacia Cart
        public Cart Cart { get; set; }  // Relación uno a uno con Cart


    }
}
