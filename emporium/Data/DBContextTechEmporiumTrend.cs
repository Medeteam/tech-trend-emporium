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
        public DbSet<Category> Categories { get; set; }
        public DbSet<JobStatus> Jobs { get; set; }
        public DbSet<ProductToCategory> ProductToCategories { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductToCart> ProductsToCart {  get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingStatus> ShoppingStatus { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación entre Product y JobStatus (sin cascada)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Job_status)
                .WithMany()
                .HasForeignKey(p => p.Job_status_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Job_status)
                .WithMany(js => js.Categories)  // Un JobStatus puede tener muchas categorías
                .HasForeignKey(c => c.Job_status_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación de clave compuesta para ProductToCart (sin cascada)
            modelBuilder.Entity<ProductToCart>()
                .HasKey(ptc => new { ptc.Product_id, ptc.Cart_id });

            modelBuilder.Entity<ProductToCart>()
                .HasOne(ptc => ptc.Product)
                .WithMany(p => p.ProductToCarts)
                .HasForeignKey(ptc => ptc.Product_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada para evitar conflictos

            modelBuilder.Entity<ProductToCart>()
                .HasOne(ptc => ptc.Cart)
                .WithMany(c => c.ProductToCarts)
                .HasForeignKey(ptc => ptc.Cart_id)
                .OnDelete(DeleteBehavior.Cascade);  // Mantener cascada

            // Relación de clave compuesta para ProductToCategory
            modelBuilder.Entity<ProductToCategory>()
                .HasKey(ptcy => new { ptcy.Product_id, ptcy.Category_id });

            // Relación de clave compuesta para ProductWishList
            modelBuilder.Entity<ProductWishList>()
                .HasKey(pwl => new { pwl.Product_id, pwl.Wishlist_id });

            // Relación entre ProductWishList y Product (sin cascada)
            modelBuilder.Entity<ProductWishList>()
                .HasOne(pwl => pwl.Product)
                .WithMany(p => p.ProductWishLists)
                .HasForeignKey(pwl => pwl.Product_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada

            // Relación entre ProductWishList y WishList (sin cascada)
            modelBuilder.Entity<ProductWishList>()
                .HasOne(pwl => pwl.WishList)
                .WithMany(wl => wl.ProductWishLists)
                .HasForeignKey(pwl => pwl.Wishlist_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada

            // Relación uno a uno entre Cart y Order (el carrito es principal)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(o => o.Cart_id)  // La clave foránea está en Orders
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre Orders y ShoppingStatus
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShoppingStatus)
                .WithMany()
                .HasForeignKey(o => o.Shopping_status_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada

            // Relación entre Orders y Users (mantener cascada)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.User_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre Products y WishList (sin cascada)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.WishList)
                .WithMany(wl => wl.Products)
                .HasForeignKey(p => p.Wishlist_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada

            // Relación entre Categories y Users sin cascada
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.User_id)
                .OnDelete(DeleteBehavior.NoAction);  // Sin cascada

            // Relación uno a uno entre User y Cart (el usuario es principal)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.User_id)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración del campo Price en Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

        }
    }

}
