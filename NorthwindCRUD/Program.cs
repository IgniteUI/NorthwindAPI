using System.Text;
using AutoMapper;
using GraphQL.AspNet.Configuration.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NorthwindCRUD.Filters;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Services;

namespace NorthwindCRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var allowAnyOriginPolicy = "_allowAnyOrigin";

            builder.Logging.ClearProviders();

            builder.Services.AddLogging(options =>
            {
                var logConfigSection = builder.Configuration.GetSection("Logging");
                options.AddConfiguration(logConfigSection);
            });

            builder.Logging.AddConsole();

            builder.Services.AddControllers(options =>
                                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                            .AddNewtonsoftJson(options =>
                                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind CRUD", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                });

                option.OperationFilter<AuthResponsesOperationFilter>();
                option.EnableAnnotations();
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                                name: allowAnyOriginPolicy,
                                policy =>
                                {
                                    policy.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                });
            });

            var dbProvider = builder.Configuration.GetConnectionString("Provider");

            if (dbProvider == "SQLite")
            {
                // For SQLite in memory to be shared across multiple EF calls, we need to maintain a separate open connection.
                // see post https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created
                var keepAliveConnection = new SqliteConnection(builder.Configuration.GetConnectionString("SQLiteConnectionString"));
                keepAliveConnection.Open();
            }

            builder.Services.AddDbContext<DataContext>(options =>
            {
                if (dbProvider == "SqlServer")
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"));
                }
                else if (dbProvider == "InMemory")
                {
                    options.ConfigureWarnings(warnOpts =>
                    {
                        // InMemory doesn't support transactions and we're ok with it
                        warnOpts.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                    });

                    options.UseInMemoryDatabase(databaseName: builder.Configuration.GetConnectionString("InMemoryDBConnectionString"));
                }
                else if (dbProvider == "SQLite")
                {
                    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString"));
                }
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var mapper = config.CreateMapper();

            builder.Services.AddSingleton(mapper);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<DBSeeder>();
            builder.Services.AddTransient<CategoryService>();
            builder.Services.AddTransient<CustomerService>();
            builder.Services.AddTransient<EmployeeTerritoryService>();
            builder.Services.AddTransient<EmployeeService>();
            builder.Services.AddTransient<OrderService>();
            builder.Services.AddTransient<AuthService>();
            builder.Services.AddTransient<ProductService>();
            builder.Services.AddTransient<RegionService>();
            builder.Services.AddTransient<ShipperService>();
            builder.Services.AddTransient<SupplierService>();
            builder.Services.AddTransient<TerritoryService>();
            builder.Services.AddTransient<SalesService>();
            builder.Services.AddTransient<IPagingService, PagingService>();

            var app = builder.Build();

            // Necessary to detect if it's behind a load balancer, for example changing protocol, port or hostname
            app.UseForwardedHeaders();

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(allowAnyOriginPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphQL();

            app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                        {
                            // Adding server base address in the generated file relative to the server's host
                            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}" } };
                        });
                });

            app.UseSwaggerUI();
            app.UseSeedDB();

            app.MapControllers();

            app.Run();
        }
    }
}
