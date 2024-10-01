using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities
{
    public class Product
    {
        [Key]
        public Guid Product_id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        [JsonPropertyName("title")]
        public required string Name { get; set; }

        [MaxLength(2000)]
        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [MaxLength(255)]
        [JsonPropertyName("image")]
        public required string Image { get; set; }

        [JsonPropertyName("price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public uint Stock { get; set; }

        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("Category")]

        public Guid Category_id { get; set; }

        [JsonPropertyName("categoryObject")]

        public Category? Category { get; set; }

        [JsonPropertyName("category")]
        [NotMapped]
        public string? CategoryName {  get; set; }

        public List<ProductWishList> ProductWishLists { get; set; }
        public List<ProductToCart> ProductToCart { get; set; }

    }
}
