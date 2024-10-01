using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ProductWishList
    {
        [ForeignKey("Product")]
        public Guid Product_id { get; set; }
        public Product? Product { get; set; }

        [ForeignKey("WishList")]
        public Guid Wishlist_id { get; set; }
        public WishList? WishList { get; set; }
    }
}
