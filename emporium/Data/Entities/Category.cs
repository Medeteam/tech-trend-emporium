using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Category_id { get; set; } = Guid.NewGuid();


        [MaxLength(255)]
        public required string  Category_name { get; set; }

        [MaxLength(2000)]
        public required string Category_description { get; set; }

        public DateTimeOffset Created_at { get; set; }

    }
}
