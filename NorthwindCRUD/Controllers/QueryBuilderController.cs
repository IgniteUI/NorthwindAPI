namespace QueryBuilder;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindCRUD;
using NorthwindCRUD.Models.Dtos;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

[SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1516:Elements should be separated by blank line", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1134:Attributes should not share line", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "...")]
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
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
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
        logger.LogInformation("Executing query for entity: {Entity}", query.Entity);
        var t = query.Entity.ToLower(CultureInfo.InvariantCulture);
        return Ok(new QueryBuilderResult
        {
            Addresses    = t == "addresses"    ? mapper.Map<AddressDto[]>(dataContext.Addresses.Run(query, dataContext))        : null,
            Categories   = t == "categories"   ? mapper.Map<CategoryDto[]>(dataContext.Categories.Run(query, dataContext))      : null,
            Products     = t == "products"     ? mapper.Map<ProductDto[]>(dataContext.Products.Run(query, dataContext))         : null,
            Regions      = t == "regions"      ? mapper.Map<RegionDto[]>(dataContext.Regions.Run(query, dataContext))           : null,
            Territories  = t == "territories"  ? mapper.Map<TerritoryDto[]>(dataContext.Territories.Run(query, dataContext))    : null,
            Employees    = t == "employees"    ? mapper.Map<EmployeeDto[]>(dataContext.Employees.Run(query, dataContext))       : null,
            Customers    = t == "customers"    ? mapper.Map<CustomerDto[]>(dataContext.Customers.Run(query, dataContext))       : null,
            Orders       = t == "orders"       ? mapper.Map<OrderDto[]>(dataContext.Orders.Run(query, dataContext))             : null,
            OrderDetails = t == "orderdetails" ? mapper.Map<OrderDetailDto[]>(dataContext.OrderDetails.Run(query, dataContext)) : null,
            Shippers     = t == "shippers"     ? mapper.Map<ShipperDto[]>(dataContext.Shippers.Run(query, dataContext))         : null,
            Suppliers    = t == "suppliers"    ? mapper.Map<SupplierDto[]>(dataContext.Suppliers.Run(query, dataContext))       : null,
        });
    }
}