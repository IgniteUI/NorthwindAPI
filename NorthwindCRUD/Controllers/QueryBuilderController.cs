namespace QueryBuilder;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindCRUD;
using NorthwindCRUD.Models.DbModels;

public class QueryBuilderResult
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AddressDb[] Addresses { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CategoryDb[] Categories { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ProductDb[] Products { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RegionDb[] Regions { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TerritoryDb[] Territories { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public EmployeeDb[] Employees { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CustomerDb[] Customers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OrderDb[] Orders { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OrderDetailDb[] OrderDetails { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ShipperDb[] Shippers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public SupplierDb[] Suppliers { get; set; }
}

[ApiController]
[Route("[controller]")]
public class QueryBuilderController : ControllerBase
{
    private readonly DataContext dataContext;
    private readonly ILogger<QueryBuilderController> logger;

    public QueryBuilderController(DataContext dataContext, ILogger<QueryBuilderController> logger)
    {
        this.dataContext = dataContext;
        this.logger = logger;
    }

    [HttpPost("ExecuteQuery")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public ActionResult<QueryBuilderResult> ExecuteQuery(Query query)
    {
        logger.LogInformation("Executing query for entity: {Entity}", query.Entity);
#pragma warning disable CS8601 // Possible null reference assignment.
        return Ok(new QueryBuilderResult
        {
            Addresses = query.Entity == "Addresses" ? dataContext.Addresses.Run(query) : null,
            Categories = query.Entity == "Categories" ? dataContext.Categories.Run(query) : null,
            Products = query.Entity == "Products" ? dataContext.Products.Run(query) : null,
            Regions = query.Entity == "Regions" ? dataContext.Regions.Run(query) : null,
            Territories = query.Entity == "Territories" ? dataContext.Territories.Run(query) : null,
            Employees = query.Entity == "Employees" ? dataContext.Employees.Run(query) : null,
            Customers = query.Entity == "Customers" ? dataContext.Customers.Run(query) : null,
            Orders = query.Entity == "Orders" ? dataContext.Orders.Run(query) : null,
            OrderDetails = query.Entity == "OrderDetails" ? dataContext.OrderDetails.Run(query) : null,
            Shippers = query.Entity == "Shippers" ? dataContext.Shippers.Run(query) : null,
            Suppliers = query.Entity == "Suppliers" ? dataContext.Suppliers.Run(query) : null,
        });
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}