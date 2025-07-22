using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NorthwindCRUD.Helpers;

namespace NorthwindCRUD.Providers
{
    public class DbContextConfigurationProvider : IDisposable
    {
        private const string DefaultTenantId = "default-tenant";
        private const string TenantHeaderKey = "X-Tenant-ID";
        private const string DatabaseConnectionCacheKey = "Data-Connection-{0}";

        private readonly IHttpContextAccessor context;
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;

        private SqliteConnection? currentRequestConnection;

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
                var tenantId = GetTenantId();
                var connectionString = this.GetSqlLiteConnectionString(tenantId);

                var cacheKey = string.Format(CultureInfo.InvariantCulture, DatabaseConnectionCacheKey, tenantId);

                if (!memoryCache.TryGetValue(cacheKey, out SqliteConnection? connection))
                {
                    // Create a cached connection to seed the database and keep the data alive
                    connection = new SqliteConnection(connectionString);
                    memoryCache.Set(cacheKey, connection, GetCacheConnectionEntryOptions());
                    connection.Open();

                    options.UseSqlite(connection).EnableSensitiveDataLogging();
                    SeedDb(options);
                }

                // Create a new connection per request to avoid threading issues
                currentRequestConnection = new SqliteConnection(connectionString);
                currentRequestConnection.Open();
                options.UseSqlite(currentRequestConnection).EnableSensitiveDataLogging();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                currentRequestConnection?.Close();
            }
        }

        private static void SeedDb(DbContextOptionsBuilder optionsBuilder)
        {
            using var dataContext = new DataContext(optionsBuilder.Options);
            DBSeeder.Seed(dataContext);
        }

        private static void CloseConnection(object key, object? value, EvictionReason reason, object? state)
        {
            //Used to clear datasource from memory.
            (value as SqliteConnection)?.Close();
        }

        private MemoryCacheEntryOptions GetCacheConnectionEntryOptions()
        {
            var defaultAbsoluteCacheExpirationInHours = this.configuration.GetValue<int>("DefaultAbsoluteCacheExpirationInHours");
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(defaultAbsoluteCacheExpirationInHours),
            };

            cacheEntryOptions.RegisterPostEvictionCallback(CloseConnection);

            return cacheEntryOptions;
        }

        private string GetSqlLiteConnectionString(string tenantId)
        {
            var connectionStringTemplate = configuration.GetConnectionString("SQLiteConnectionString")
                ?? throw new InvalidOperationException("SQLiteConnectionString not found");
            var unsanitizedConntectionString = string.Format(CultureInfo.InvariantCulture, connectionStringTemplate, tenantId);
            var connectionStringBuilder = new SqliteConnectionStringBuilder(unsanitizedConntectionString);
            var sanitizedConntectionString = connectionStringBuilder.ToString();

            return sanitizedConntectionString;
        }

        private string GetTenantId()
        {
            return context.HttpContext?.Request.Headers[TenantHeaderKey].FirstOrDefault() ?? DefaultTenantId;
        }
    }
}
