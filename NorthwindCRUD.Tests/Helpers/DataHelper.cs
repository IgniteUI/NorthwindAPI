using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Tests
{
    public class DataHelper
    {
        private DataContext dataContext;
        private IMapper mapper;

        public DataHelper(DataContext dataContext, IPagingService pagingService, IMapper mapper)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
            CustomerService = new CustomerService(dataContext, pagingService, mapper);
            EmployeeService = new EmployeeService(dataContext, pagingService, mapper);
            ProductService = new ProductService(dataContext, pagingService, mapper);
            SupplierService = new SupplierService(dataContext, pagingService, mapper);
            RegionService = new RegionService(dataContext, pagingService, mapper);
            TerritoryService = new TerritoryService(dataContext, pagingService, mapper);
            OrderService = new OrderService(dataContext, pagingService, mapper);
            ShipperService = new ShipperService(dataContext, pagingService, mapper);
            EmployeeTerritoryService = new EmployeeTerritoryService(dataContext, mapper);
            CategoryService = new CategoryService(dataContext, pagingService, mapper);
            SalesService = new SalesService(dataContext);
        }

        public CategoryService CategoryService { get; set; }

        public CustomerService CustomerService { get; set; }

        public EmployeeService EmployeeService { get; set; }

        public ProductService ProductService { get; set; }

        public SupplierService SupplierService { get; set; }

        public TerritoryService TerritoryService { get; set; }

        public OrderService OrderService { get; set; }

        public SalesService SalesService { get; set; }

        public ShipperService ShipperService { get; set; }

        public RegionService RegionService { get; set; }

        public EmployeeTerritoryService EmployeeTerritoryService { get; set; }

        internal CategoryDto GetCategory()
        {
            var categores = GetJsonContent<CategoryDb>("categories.json").GetRandomElement();
            return mapper.Map<CategoryDto>(categores);
        }

        internal CustomerDto GetCustomer()
        {
            var customer = GetJsonContent<CustomerDb>("customers.json").GetRandomElement();
            return mapper.Map<CustomerDto>(customer);
        }

        internal EmployeeDto GetEmployee()
        {
            var employee = GetJsonContent<EmployeeDb>("employees.json").GetRandomElement();
            return mapper.Map<EmployeeDto>(employee);
        }

        internal ProductDto GetProduct()
        {
            var product = GetJsonContent<ProductDb>("products.json").GetRandomElement();
            return mapper.Map<ProductDto>(product);
        }

        internal SupplierDto GetSupplier()
        {
            var supplier = GetJsonContent<SupplierDb>("suppliers.json").GetRandomElement();
            return mapper.Map<SupplierDto>(supplier);
        }

        internal ShipperDto GetShipper()
        {
            var shipper = GetJsonContent<ShipperDb>("shippers.json").GetRandomElement();
            return mapper.Map<ShipperDto>(shipper);
        }

        internal TerritoryDto GetTerritory()
        {
            var territory = GetJsonContent<TerritoryDb>("territories.json").GetRandomElement();
            return mapper.Map<TerritoryDto>(territory);
        }

        internal RegionDto GetRegion()
        {
            var region = GetJsonContent<RegionDb>("regions.json").GetRandomElement();
            return mapper.Map<RegionDto>(region);
        }

        internal OrderDto GetOrder()
        {
            var order = GetJsonContent<OrderDb>("orders.json").GetRandomElement();
            return mapper.Map<OrderDto>(order);
        }

        internal OrderDetailDto GetOrderDetail()
        {
            var orderDetail = GetJsonContent<OrderDetailDb>("orderDetails.json").GetRandomElement();
            return mapper.Map<OrderDetailDto>(orderDetail);
        }

        internal async Task<SupplierDto> CreateSupplier()
        {
            var supplier = GetSupplier();
            var createdSupplier = await SupplierService.Create(supplier);
            dataContext.Entry(mapper.Map<SupplierDb>(createdSupplier)).State = EntityState.Detached;
            return createdSupplier;
        }

        internal async Task<ShipperDto> CreateShipper()
        {
            var shipper = GetShipper();
            var createdShipper = await ShipperService.Create(shipper);
            dataContext.Entry(mapper.Map<ShipperDb>(createdShipper)).State = EntityState.Detached;
            return createdShipper;
        }

        internal async Task<RegionDto> CreateRegion()
        {
            var region = GetRegion();
            var createdRegion = await RegionService.Create(region);
            dataContext.Entry(mapper.Map<RegionDb>(createdRegion)).State = EntityState.Detached;
            return createdRegion;
        }

        internal async Task<CustomerDto> CreateCustomer()
        {
            var customer = GetCustomer();
            var createdCustomer = await CustomerService.Create(customer);
            dataContext.Entry(mapper.Map<CustomerDb>(createdCustomer)).State = EntityState.Detached;
            return createdCustomer;
        }

        internal async Task<OrderDto> CreateOrder(string? orderDate = null, string? country = null, ProductDto? product = null, int? quantity = null)
        {
            var order = GetOrder();
            var customer = await CreateCustomer();
            var employee = await CreateEmployee();
            var shipper = await CreateShipper();
            order.CustomerId = customer.CustomerId;
            order.EmployeeId = employee.EmployeeId;
            order.ShipperId = shipper.ShipperId;

            if (orderDate != null)
            {
                order.OrderDate = orderDate;
            }

            if (country != null)
            {
                order.ShipAddress = new AddressDto
                {
                    Country = country,
                    Street = string.Empty,
                    City = string.Empty,
                    Region = string.Empty,
                    PostalCode = string.Empty,
                };
            }

            var result = await OrderService.Create(order);
            var details = GetOrderDetail();
            details.OrderId = result.OrderId;

            if (product == null)
            {
                product = await CreateProduct();
            }

            if (quantity != null)
            {
                details.Quantity = quantity.Value;
            }

            details.ProductId = product.ProductId;

            this.dataContext.Add(mapper.Map<OrderDetailDb>(details));
            this.dataContext.SaveChanges();

            return result;
        }

        internal async Task<ProductDto> CreateProduct(ProductDto? product = null)
        {
            if (product == null)
            {
                product = GetProduct();
            }

            var createdCategory = await CategoryService.Create(GetCategory());
            product.CategoryId = createdCategory.CategoryId;

            var createdSupplier = await SupplierService.Create(GetSupplier());
            product.SupplierId = createdSupplier.SupplierId;

            return await ProductService.Create(product);
        }

        internal async Task<EmployeeDto> CreateEmployee()
        {
            return await EmployeeService.Create(GetEmployee());
        }

        internal async Task<TerritoryDto> CreateTerritory(TerritoryDto? territory = null)
        {
            if (territory == null)
            {
                territory = GetTerritory();
            }

            var region = await CreateRegion();
            territory.RegionId = region.RegionId;
            return await TerritoryService.Create(territory);
        }

        internal EmployeeTerritoryDto CreateEmployeeTerritory(int employeeId, string territoryId)
        {
            var employeeTerritory = new EmployeeTerritoryDto
            {
                EmployeeId = employeeId,
                TerritoryId = territoryId,
            };

            return EmployeeTerritoryService.AddTerritoryToEmployee(employeeTerritory);
        }

        private static T[] GetJsonContent<T>(string jsonFile)
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (baseDir == null)
            {
                throw new DirectoryNotFoundException($"{Assembly.GetExecutingAssembly().Location} folder not found");
            }

            string jsonData = File.ReadAllText(Path.Combine(baseDir, "Resources", jsonFile));
            var result = JsonConvert.DeserializeObject<T[]>(jsonData);

            if (result == null)
            {
                throw new InvalidOperationException($"{typeof(T)} not found");
            }

            return result;
        }
    }
}
