using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Tests
{
    public class BaseFixture
    {
        // null! indicates that the member is initialized in other code
        private DbConnection? connection = null!;

        protected DataContext GetInMemoryDatabaseContext()
        {
            if (connection == null)
            {
                connection = CreateDbConnection();
                var context = CreateInMemoryDatabaseContext(connection);
                DBSeeder.Seed(context);
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
