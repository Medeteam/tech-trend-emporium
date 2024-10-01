using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DBContextTechEmporiumTrend:DbContext
    {
        public DBContextTechEmporiumTrend(DbContextOptions<DBContextTechEmporiumTrend> options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductToCart> ProductsToCart {  get; set; }
        public DbSet<ProductWishList> ProductWishLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingStatus> ShoppingStatus { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductWishList>()
                .HasKey(pw => new { pw.Wishlist_id, pw.Product_id });

            modelBuilder.Entity<ProductToCart>()
                .HasKey(pc => new { pc.Cart_id, pc.Product_id });
        }
    }

}
