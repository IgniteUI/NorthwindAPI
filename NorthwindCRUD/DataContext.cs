
namespace NorthwindCRUD
{
    using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressDb>()
                .Property(a => a.AddressId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CustomerDb>()
                .HasOne(c => c.Address)
                .WithMany(a => a.Customers);

            modelBuilder.Entity<EmployeeDb>()
               .HasOne(e => e.Address)
               .WithMany(a => a.Employees);

            modelBuilder.Entity<OrderDb>()
               .HasOne(o => o.ShipAddress)
               .WithMany(a => a.Orders);
        }

    }
}
