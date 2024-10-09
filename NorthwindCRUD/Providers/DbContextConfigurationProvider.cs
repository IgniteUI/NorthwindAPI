using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NorthwindCRUD.Helpers;

namespace NorthwindCRUD.Providers
{
    public class DbContextConfigurationProvider
    {
        private const string DefaultTenantId = "default-tenant";
        private const string TenantHeaderKey = "X-Tenant-ID";
        private const string DatabaseConnectionCacheKey = "Data-Connection-{0}";

        private readonly TimeSpan connectionKeySlidingExpiration = TimeSpan.FromHours(24);

        private readonly IHttpContextAccessor context;
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;

        public DbContextConfigurationProvider(IHttpContextAccessor context, IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.context = context;
            this.memoryCache = memoryCache;
            this.configuration = configuration;
        }

        public void ConfigureOptions(DbContextOptionsBuilder options)
        {
            var dbProvider = configuration.GetConnectionString("Provider");

            if (dbProvider == "SqlServer")
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString"));
            }
            else if (dbProvider == "SQLite")
            {
                var tenantId = context.HttpContext?.Request.Headers[TenantHeaderKey].FirstOrDefault() ?? DefaultTenantId;
                var connectionString = string.Format(configuration.GetConnectionString("SQLiteConnectionString"), tenantId);

                if (!memoryCache.TryGetValue(string.Format(DatabaseConnectionCacheKey, tenantId), out SqliteConnection connection))
                {
                    connection = new SqliteConnection(connectionString);
                    var cacheKey = string.Format(DatabaseConnectionCacheKey, tenantId);
                    memoryCache.Set(cacheKey, connection, GetCacheConnectionEntryOptions());
                }

                // For SQLite in memory to be shared across multiple EF calls, we need to maintain a separate open connection.
                // see post https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created
                connection.Open();

                options.UseSqlite(connection).EnableSensitiveDataLogging();

                SeedDb(options);
            }
        }

        private static void SeedDb(DbContextOptionsBuilder optionsBuilder)
        {
            using var dataContext = new DataContext(optionsBuilder.Options);
            DBSeeder.Seed(dataContext);
        }

        private static void CloseConnection(object key, object value, EvictionReason reason, object state)
        {
            //Used to clear datasource from memory.
            (value as SqliteConnection)?.Close();
        }

        private MemoryCacheEntryOptions GetCacheConnectionEntryOptions()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = this.connectionKeySlidingExpiration,
            };

            cacheEntryOptions.RegisterPostEvictionCallback(CloseConnection);

            return cacheEntryOptions;
        }
    }
}
