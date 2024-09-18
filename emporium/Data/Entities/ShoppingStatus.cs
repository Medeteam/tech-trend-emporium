using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ShoppingStatus
    {
        [Key]
        public Guid Shopping_status_id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public required string Shopping_status { get; set; }
    }
}
