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

        public DbSet<AddressDb> Addresses { get; set; }

        public DbSet<AssetDb> Assets { get; set; }

        public DbSet<BrandSaleDb> BrandSales { get; set; }

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

        public DbSet<VehicleDetailDb> VehicleDetails { get; set; }

        public DbSet<VehicleDb> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressDb>()
                .Property(a => a.AddressId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductDb>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProductDb>()
               .HasOne(p => p.Supplier)
               .WithMany(c => c.Products)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CustomerDb>()
                .HasOne(c => c.Address)
                .WithMany(a => a.Customers)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDb>()
                .HasOne(o => o.ShipAddress)
                .WithMany(a => a.Orders)
                .OnDelete(DeleteBehavior.SetNull);

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
                .HasOne(e => e.Address)
                .WithMany(a => a.Employees)
                .OnDelete(DeleteBehavior.NoAction);

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

            modelBuilder.Entity<VehicleDetailDb>()
                .Property(p => p.DetailId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<VehicleDb>()
                .HasOne(v => v.Details)
                .WithOne(vd => vd.Vehicle)
                .HasForeignKey<VehicleDetailDb>(vd => vd.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            //Composite Keys
            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasKey(et => new { et.EmployeeId, et.TerritoryId });

            modelBuilder.Entity<OrderDetailDb>()
                .HasKey(et => new { et.ProductId, et.OrderId });
        }
    }
}
