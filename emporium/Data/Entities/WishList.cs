using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities
{
    public class WishList
    {
        [Key]
        public Guid Wishlist_id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;


        [ForeignKey("User")] //Es 1 a 1 se deja
        public Guid User_id { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public List<ProductWishList> ProductWishLists { get; set; } = new List<ProductWishList>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
