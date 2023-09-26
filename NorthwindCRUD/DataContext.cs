namespace NorthwindCRUD
{
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Models.Contracts;
    using NorthwindCRUD.Models.DbModels;

    public class DataContext :  DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AddressDb> Addresses { get; set; }
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
            modelBuilder.Entity<AddressDb>()
                .Property(a => a.AddressId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductDb>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products);

            modelBuilder.Entity<ProductDb>()
              .HasOne(p => p.Supplier)
              .WithMany(c => c.Products);

            modelBuilder.Entity<CustomerDb>()
                .HasOne(c => c.Address)
                .WithMany(a => a.Customers);

            modelBuilder.Entity<EmployeeDb>()
               .HasOne(e => e.Address)
               .WithMany(a => a.Employees);

            modelBuilder.Entity<OrderDb>()
               .HasOne(o => o.ShipAddress)
               .WithMany(a => a.Orders);

            modelBuilder.Entity<OrderDb>()
              .HasOne(o => o.Shipper)
              .WithMany(s => s.Orders);

            modelBuilder.Entity<OrderDetailDb>()
               .HasOne(od => od.Product)
               .WithMany(p => p.Details);

            modelBuilder.Entity<OrderDetailDb>()
                .HasOne(o => o.Order)
                .WithMany(o => o.Details);

            modelBuilder.Entity<TerritoryDb>()
                .HasOne(t => t.Region)
                .WithMany(r => r.Territories);

            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasOne(et => et.Employee)
                .WithMany(e => e.EmployeesTerritories);

            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasOne(et => et.Territory)
                .WithMany(t => t.EmployeesTerritories);

            //Composite Keys

            modelBuilder.Entity<EmployeeTerritoryDb>()
                .HasKey(et => new { et.EmployeeId, et.TerritoryId });

            modelBuilder.Entity<OrderDetailDb>()
               .HasKey(et => new { et.ProductId, et.OrderId });
        }
    }
}
