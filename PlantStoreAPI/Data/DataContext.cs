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
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherApplied> VoucherApplied { get; set; }
        public DbSet<WishList> WishLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoucherApplied>().HasKey(v => new { v.VoucherID, v.CustomerID });
            modelBuilder.Entity<WishList>().HasKey(v => new { v.CustomerID, v.ProductID });
            base.OnModelCreating(modelBuilder);
        }
    }
}
