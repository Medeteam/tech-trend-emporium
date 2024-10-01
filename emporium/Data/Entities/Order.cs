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
    }
}
