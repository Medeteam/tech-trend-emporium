using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Product
    {
        [Key]
        public Guid Product_id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public required string Description { get; set; }

        [MaxLength(255)]
        public required string Image { get; set; }

        public uint Price { get; set; }

        public uint Stock { get; set; }

        public DateTimeOffset Created_at { get; set; } = DateTimeOffset.Now;

        [ForeignKey("User")]
        public Guid User_id { get; set; }
        public User User { get; set; }

        [ForeignKey("JobStatus")]
        public Guid Job_status_id { get; set; }
        public JobStatus Job_status { get; set; }

        [ForeignKey("WishList")]
        public Guid Wishlist_id { get; set; }
        public WishList WishList { get; set; }

        public ProductToCategory ProductToCategory { get; set; }

        public List<ProductToCart> ProductToCarts { get; set; }

        public List<ProductWishList> ProductWishLists { get; set; }

    }
}
