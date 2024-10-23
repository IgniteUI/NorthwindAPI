namespace NorthwindCRUD
{
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Models.DbModels;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<CategoryDb> Categories { get; set; }

        public DbSet<CustomerDb> Customers { get; set; }

        public DbSet<EmployeeDb> Employees { get; set; }

        public DbSet<OrderDb> Orders { get; set; }

        public DbSet<OrderDetailDb> OrderDetails { get; set; }

        public DbSet<UserDb> Users { get; set; }

        public DbSet<ProductDb> Products { get; set; }

        public DbSet<RegionDb> Regions { get; set; }

        public DbSet<ShipperDb> Shippers { get; set; }

        public DbSet<SupplierDb> Suppliers { get; set; }

        public DbSet<TerritoryDb> Territories { get; set; }

        public DbSet<EmployeeTerritoryDb> EmployeesTerritories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDb>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProductDb>()
               .HasOne(p => p.Supplier)
               .WithMany(c => c.Products)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CustomerDb>()
                .OwnsOne(c => c.Address);

            modelBuilder.Entity<OrderDb>()
                .OwnsOne(o => o.ShipAddress);

            modelBuilder.Entity<OrderDb>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderDb>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderDb>()
                .HasOne(o => o.Shipper)
                .WithMany(s => s.Orders)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EmployeeDb>()
                .OwnsOne(e => e.Address);

            modelBuilder.Entity<TerritoryDb>()
                .HasOne(t => t.Region)
                .WithMany(r => r.Territories)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasOne(et => et.Employee)
                .WithMany(e => e.EmployeesTerritories);

            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasOne(et => et.Territory)
                .WithMany(t => t.EmployeesTerritories);

            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasKey(et => new { et.EmployeeId, et.TerritoryId });

            modelBuilder.Entity<OrderDetailDb>()
                .HasKey(et => new { et.ProductId, et.OrderId });
        }
    }
}
