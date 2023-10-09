﻿using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Tests
{
    public class DataHelper
    {
        public DataHelper(DataContext context)
        {
            CategoryService = new CategoryService(context);
            CustomerService = new CustomerService(context);
            EmployeeService = new EmployeeService(context);
            ProductService = new ProductService(context);
            SupplierService = new SupplierService(context);
            RegionService = new RegionService(context);
            TerritoryService = new TerritoryService(context);
            EmployeeTerritoryService = new EmployeeTerritoryService(context);
        }

        public CategoryService CategoryService { get; set; }

        public CustomerService CustomerService { get; set; }

        public EmployeeService EmployeeService { get; set; }

        public ProductService ProductService { get; set; }

        public SupplierService SupplierService { get; set; }

        public TerritoryService TerritoryService { get; set; }

        public RegionService RegionService { get; set; }

        public EmployeeTerritoryService EmployeeTerritoryService { get; set; }

        internal static CategoryDb GetCategory()
        {
            return GetJsonContent<CategoryDb>("categories.json").GetRandomElement();
        }

        internal static CustomerDb GetCustomer()
        {
            return GetJsonContent<CustomerDb>("customers.json").GetRandomElement();
        }

        internal static EmployeeDb GetEmployee()
        {
            return GetJsonContent<EmployeeDb>("employees.json").GetRandomElement();
        }

        internal static ProductDb GetProduct()
        {
            return GetJsonContent<ProductDb>("products.json").GetRandomElement();
        }

        internal static SupplierDb GetSupplier()
        {
            return GetJsonContent<SupplierDb>("suppliers.json").GetRandomElement();
        }

        internal static TerritoryDb GetTerritory()
        {
            return GetJsonContent<TerritoryDb>("territories.json").GetRandomElement();
        }

        internal static RegionDb GetRegion()
        {
            return GetJsonContent<RegionDb>("regions.json").GetRandomElement();
        }

        internal RegionDb CreateRegion()
        {
            var region = GetRegion();
            return RegionService.Create(region);
        }

        internal ProductDb CreateProduct(ProductDb product)
        {
            var createdCategory = CategoryService.Create(DataHelper.GetCategory());
            product.CategoryId = createdCategory.CategoryId;

            var createdSupplier = SupplierService.Create(DataHelper.GetSupplier());
            product.SupplierId = createdSupplier.SupplierId;

            return ProductService.Create(product);
        }

        internal EmployeeDb CreateEmployee()
        {
            var createdEmployee = EmployeeService.Create(GetEmployee());
            return createdEmployee;
        }

        internal TerritoryDb CreateTerritory()
        {
            TerritoryDb territory = GetTerritory();
            return CreateTerritory(territory);
        }

        internal TerritoryDb CreateTerritory(TerritoryDb territory)
        {
            RegionDb region = RegionService.Create(GetRegion());
            territory.RegionId = region.RegionId;
            return TerritoryService.Create(territory);
        }

        internal EmployeeTerritoryDb CreateEmployeeTerritory(int employeeId, string territoryId)
        {
            var employeeTerritory = new EmployeeTerritoryDb
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
