using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class WishList
    {
        [Key]
        public Guid Wishlist_id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        public List<ProductWishList> WishLists { get; set; }
    }
}
