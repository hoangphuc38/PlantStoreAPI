using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantStoreAPI.Model;

namespace PlantStoreAPI.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<DeliveryInfo> DeliveryInfos { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherApplied> VoucherApplied { get; set; }
        public DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(v => new { v.CustomerID, v.ProductID });
            modelBuilder.Entity<OrderDetail>().HasKey(v => new { v.OrderID, v.ProductID });
            modelBuilder.Entity<Feedback>().HasKey(v => new { v.CustomerID, v.ProductID });
            modelBuilder.Entity<VoucherApplied>().HasKey(v => new { v.VoucherID, v.CustomerID });
            modelBuilder.Entity<WishList>().HasKey(v => new { v.CustomerID, v.ProductID });
            base.OnModelCreating(modelBuilder);
        }
    }
}
