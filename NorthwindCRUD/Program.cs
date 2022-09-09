using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"));
});

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<ControllerBase>>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSeedDB();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
