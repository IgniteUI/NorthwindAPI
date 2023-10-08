using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Helpers;
using System.Data.Common;

namespace NorthwindCRUD.Tests
{
    public class BaseFixture
    {
        private DbConnection? connection;

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

        protected static DbConnection CreateDbConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }


        protected static DataContext CreateInMemoryDatabaseContext(DbConnection connection)
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
