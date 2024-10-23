using System.Data.Common;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Tests
{
    public class BaseFixture
    {
        // null! indicates that the member is initialized in other code
        private DbConnection? connection = null!;

        public DataHelper DataHelper { get; set; } = null!;

        public DataHelper DataHelper2 { get; set; } = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mappingConfigs = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            var mapper = mappingConfigs.CreateMapper();
            var pagingService = new PagingService(mapper);
            DataContext context = GetInMemoryDatabaseContext();
            DataContext context2 = GetInMemoryDatabaseContext();
            Assert.AreNotEqual(context.GetHashCode(), context2.GetHashCode(), "Contexts instances should be different.");
            Assert.AreEqual(context.Database.GetDbConnection(), context2.Database.GetDbConnection(), "Contexts instances should have the same database connection.");

            DataHelper = new DataHelper(context, pagingService, mapper);
            DataHelper2 = new DataHelper(context2, pagingService, mapper);
        }

        protected DataContext GetInMemoryDatabaseContext()
        {
            if (connection == null)
            {
                connection = CreateDbConnection();
                var context = CreateInMemoryDatabaseContext(connection);
                context.Database.EnsureCreated();

                // DBSeeder.Seed(context);
                return context;
            }
            else
            {
                // Create a new Context on already initialized DB connection
                return CreateInMemoryDatabaseContext(connection);
            }
        }

        protected DbConnection CreateDbConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }

        protected DataContext CreateInMemoryDatabaseContext(DbConnection connection)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()

                // Uncomment the following line for detailed sql EF logs.
                // .EnableDetailedErrors().LogTo(Console.WriteLine, LogLevel.Debug)
                .Options;

            return new DataContext(options);
        }
    }
}
