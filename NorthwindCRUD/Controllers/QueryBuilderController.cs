namespace QueryBuilder;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindCRUD;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

[SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1516:Elements should be separated by blank line", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1134:Attributes should not share line", Justification = "...")]
public class QueryBuilderResult
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public AddressDto[]?     Addresses    { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public CategoryDto[]?    Categories   { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ProductDto[]?     Products     { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public RegionDto[]?      Regions      { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public TerritoryDto[]?   Territories  { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public EmployeeDto[]?    Employees    { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public CustomerDto[]?    Customers    { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public OrderDto[]?       Orders       { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public OrderDetailDto[]? OrderDetails { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ShipperDto[]?     Shippers     { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public SupplierDto[]?    Suppliers    { get; set; }
}

[ApiController]
[Route("[controller]")]
public class QueryBuilderController : ControllerBase
{
    private readonly DataContext dataContext;
    private readonly IMapper mapper;
    private readonly ILogger<QueryBuilderController> logger;

    public QueryBuilderController(DataContext dataContext, IMapper mapper, ILogger<QueryBuilderController> logger)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpPost("ExecuteQuery")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public ActionResult<QueryBuilderResult> ExecuteQuery(Query query)
    {
        var sanitizedEntity = query.Entity.Replace("\r", string.Empty).Replace("\n", string.Empty);
        logger.LogInformation("Executing query for entity: {Entity}", sanitizedEntity);
        var t = query.Entity.ToLower(CultureInfo.InvariantCulture);
        return Ok(new Dictionary<string, object[]?>
        {
            {
                t,
                t switch
                {
                    "addresses" => dataContext.Addresses.Run<AddressDb, AddressDto>(query, mapper),
                    "categories" => dataContext.Categories.Run<CategoryDb, CategoryDto>(query, mapper),
                    "products" => dataContext.Products.Run<ProductDb, ProductDto>(query, mapper),
                    "regions" => dataContext.Regions.Run<RegionDb, RegionDto>(query, mapper),
                    "territories" => dataContext.Territories.Run<TerritoryDb, TerritoryDto>(query, mapper),
                    "employees" => dataContext.Employees.Run<EmployeeDb, EmployeeDto>(query, mapper),
                    "customers" => dataContext.Customers.Run<CustomerDb, CustomerDto>(query, mapper),
                    "orders" => dataContext.Orders.Run<OrderDb, OrderDto>(query, mapper),
                    "orderdetails" => dataContext.OrderDetails.Run<OrderDetailDb, OrderDetailDto>(query, mapper),
                    "shippers" => dataContext.Shippers.Run<ShipperDb, ShipperDto>(query, mapper),
                    "suppliers" => dataContext.Suppliers.Run<SupplierDb, SupplierDto>(query, mapper),
                    _ => throw new InvalidOperationException($"Unknown entity {t}"),
                }
            },
        });
    }
}
