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
        public DbSet<JobStatus> Jobs { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductToCart> ProductsToCart {  get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingStatus> ShoppingStatus { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones de las entidades.
            ConfigureUserEntity(modelBuilder);
            ConfigureProductEntity(modelBuilder);
            ConfigureReviewEntity(modelBuilder);
            ConfigureOrderEntity(modelBuilder);
            ConfigureCategoryEntity(modelBuilder);
            ConfigureWishListEntity(modelBuilder);
            ConfigureCartEntity(modelBuilder);
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            // Relación entre User y Role (cada usuario tiene un rol)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.Role_id)
                .OnDelete(DeleteBehavior.Restrict); // No se elimina el rol si se elimina un usuario.

            // Relación uno a uno entre User y Cart (cada usuario tiene un carrito)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.User_id)
                .OnDelete(DeleteBehavior.Cascade); // Se elimina el carrito si se elimina el usuario.

            // Relación uno a uno entre User y WishList
            modelBuilder.Entity<User>()
                .HasOne(u => u.WishList)
                .WithOne(w => w.User)
                .HasForeignKey<WishList>(w => w.User_id)
                .OnDelete(DeleteBehavior.Cascade); // Se elimina la lista de deseos si se elimina el usuario.

            // Relación uno a muchos entre User y Review
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.User_id)
                .OnDelete(DeleteBehavior.Restrict); // Se eliminan las reseñas si se elimina el usuario.

            // Relación entre Review y User (evita cascada múltiple, usa Restrict o SetNull)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.User_id)
                .OnDelete(DeleteBehavior.Restrict);  // Cambia a Restrict para evitar ciclos de eliminación.
        }

        private void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            // Relación entre Product y JobStatus
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Job_status)
                .WithMany()
                .HasForeignKey(p => p.Job_status_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Product y Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.Category_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Product y User (cada producto tiene un usuario)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.User_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Product y Review
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.Product_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Product y WishList (opcional)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.WishList)
                .WithMany(w => w.Products)
                .HasForeignKey(p => p.Wishlist_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración del campo Price en Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }

        private void ConfigureReviewEntity(ModelBuilder modelBuilder)
        {
            // Relación entre Review y Product
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.Product_id)
                .OnDelete(DeleteBehavior.Cascade); // Al eliminar un producto, sus reseñas se eliminan.

            // Relación entre Review y User
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.User_id)
                .OnDelete(DeleteBehavior.Cascade); // Al eliminar un usuario, sus reseñas se eliminan.
        }

        private void ConfigureOrderEntity(ModelBuilder modelBuilder)
        {
            // Relación entre Order y Cart (uno a uno)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(o => o.Cart_id)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina un carrito, se elimina el pedido.

            // Relación entre Order y User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.User_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Order y ShoppingStatus
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShoppingStatus)
                .WithMany()
                .HasForeignKey(o => o.Shopping_status_id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureCategoryEntity(ModelBuilder modelBuilder)
        {
            // Relación entre Category y User (cada categoría puede tener un usuario)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.User_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Job_status)
                .WithMany(js => js.Categories)  // JobStatus tiene muchas categorías.
                .HasForeignKey(c => c.Job_status_id)
                .OnDelete(DeleteBehavior.Restrict);  // Eliminar sin cascada.
        }

        private void ConfigureWishListEntity(ModelBuilder modelBuilder)
        {
            // Relación entre WishList y ProductWishList
            modelBuilder.Entity<ProductWishList>()
                .HasKey(pwl => new { pwl.Product_id, pwl.Wishlist_id });

            // Relación entre ProductWishList y WishList
            modelBuilder.Entity<ProductWishList>()
                .HasOne(pwl => pwl.WishList)
                .WithMany(wl => wl.ProductWishLists)
                .HasForeignKey(pwl => pwl.Wishlist_id)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCartEntity(ModelBuilder modelBuilder)
        {
            // Relación de clave compuesta para ProductToCart
            modelBuilder.Entity<ProductToCart>()
                .HasKey(ptc => new { ptc.Product_id, ptc.Cart_id });

            // Relación entre ProductToCart y Product
            modelBuilder.Entity<ProductToCart>()
                .HasOne(ptc => ptc.Product)
                .WithMany(p => p.ProductToCarts)
                .HasForeignKey(ptc => ptc.Product_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre ProductToCart y Cart
            modelBuilder.Entity<ProductToCart>()
                .HasOne(ptc => ptc.Cart)
                .WithMany(c => c.ProductToCarts)
                .HasForeignKey(ptc => ptc.Cart_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
