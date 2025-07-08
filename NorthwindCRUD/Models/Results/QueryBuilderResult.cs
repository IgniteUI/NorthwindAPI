using Newtonsoft.Json;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Models
{
    public class QueryBuilderResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AddressDto[] Addresses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CategoryDto[] Categories { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ProductDto[] Products { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RegionDto[] Regions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TerritoryDto[] Territories { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public EmployeeDto[] Employees { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomerDto[] Customers { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OrderDto[] Orders { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OrderDetailDto[] OrderDetails { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ShipperDto[] Shippers { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SupplierDto[] Suppliers { get; set; }
    }
}
