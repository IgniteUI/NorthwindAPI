using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GraphQL.AspNet.Configuration.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NorthwindCRUD;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Services;
using System.Text;

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
        Scheme = "bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
        options.UseInMemoryDatabase(databaseName: builder.Configuration.GetConnectionString("InMemoryDBConnectionString"));
    }
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
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<DBSeeder>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<CustomerService>();
builder.Services.AddTransient<EmployeeService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<AuthService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(AllowAnyOriginPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseGraphQL();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    if (dbProvider != "InMemory")
    {
        app.UseSeedDB();
    }
}

app.MapControllers();

app.Run();
