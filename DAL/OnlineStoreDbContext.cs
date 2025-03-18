using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class OnlineStoreDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {

        public OnlineStoreDbContext(DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(OnlineStoreDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
