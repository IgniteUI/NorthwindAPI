using AutoMapper;
using GraphQL.AspNet.Configuration.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NorthwindCRUD;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Services;

var builder = WebApplication.CreateBuilder(args);
var AllowAnyOriginPolicy = "_allowAnyOrigin";

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers(options => 
                    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGraphQL();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAnyOriginPolicy,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
});

var dbProvider = builder.Configuration.GetConnectionString("Provider");

builder.Services.AddDbContext<DataContext>(options =>
{

    if (dbProvider == "SqlServer")
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"));
    }
    else if (dbProvider == "InMemory")
    {
        options.ConfigureWarnings(warnOpts => {
            // InMemory doesn't support transactions and we're ok with it
            warnOpts.Ignore(InMemoryEventId.TransactionIgnoredWarning);
        });

        options.UseInMemoryDatabase(databaseName: builder.Configuration.GetConnectionString("InMemoryDBConnectionString"));
    }
});

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<ControllerBase>>();
builder.Services.AddSingleton(typeof(ILogger), logger);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfiles());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<DBSeeder>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<CustomerService>();
builder.Services.AddTransient<EmployeeService>();
builder.Services.AddTransient<OrderService>();

var app = builder.Build();

// Necessary to detect if it's behind a load balancer, for example changing protocol, port or hostname
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(AllowAnyOriginPolicy);

app.UseAuthorization();

app.UseGraphQL();

app.UseSwagger();
app.UseSwaggerUI();
app.UseSeedDB();

app.MapControllers();

app.Run();
