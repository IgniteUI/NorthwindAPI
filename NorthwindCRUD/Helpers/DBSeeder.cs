namespace NorthwindCRUD.Helpers
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using NorthwindCRUD.Models.DbModels;

    public class DBSeeder
    {
        public static void Seed(DataContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();

            var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                SeedCategories(dbContext);
                SeedRegions(dbContext);
                SeedTerritories(dbContext);
                SeedSuppliers(dbContext); 
                SeedProducts(dbContext);
                SeedShippers(dbContext);                
                SeedEmployees(dbContext);
                SeedCustomers(dbContext);
                SeedOrders(dbContext);
                SeedOrderDetails(dbContext);
                SeedEmployeesTerritories(dbContext);

                transaction.Commit();
            }
            catch (Exception error)
            {
                transaction.Rollback();
                throw;
            }
        }

        private static void SeedEmployeesTerritories(DataContext dbContext)
        {
            if (!dbContext.EmployeesTerritories.Any())
            {
                var employeesTerritoriesData = File.ReadAllText("./Resources/employees-territories.json");
                var parsedEmployeesTerritories = JsonConvert.DeserializeObject<EmployeeTerritoryDb[]>(employeesTerritoriesData);

                dbContext.EmployeesTerritories.AddRange(parsedEmployeesTerritories);
                dbContext.SaveChanges();
            }
        }

        private static void SeedSuppliers(DataContext dbContext)
        {
            if (!dbContext.Suppliers.Any())
            {
                var suppliersData = File.ReadAllText("./Resources/suppliers.json");
                var parsedSuppliers = JsonConvert.DeserializeObject<SupplierDb[]>(suppliersData);

                foreach (var supplier in parsedSuppliers)
                {
                    var matchingProducts = dbContext.Products.Where(p => p.SupplierId == supplier.SupplierId).ToList();
                    supplier.Products = matchingProducts;

                    dbContext.Suppliers.Add(supplier);
                }

                dbContext.SaveChanges();
            }
        }

        private static void SeedShippers(DataContext dbContext)
        {
            if (!dbContext.Shippers.Any())
            {
                var shippersData = File.ReadAllText("./Resources/shippers.json");
                var parsedShippers = JsonConvert.DeserializeObject<ShipperDb[]>(shippersData);
                foreach (var shipper in parsedShippers)
                {
                    var matchingOrders = dbContext.Orders.Where(o => o.ShipperId == shipper.ShipperId).ToList();
                    shipper.Orders = matchingOrders;

                    dbContext.Shippers.Add(shipper);
                }

                dbContext.SaveChanges();
            }
        }

        private static void SeedTerritories(DataContext dbContext)
        {
            if (!dbContext.Territories.Any())
            {
                var territoriesData = File.ReadAllText("./Resources/territories.json");
                var parsedTerritories = JsonConvert.DeserializeObject<TerritoryDb[]>(territoriesData);

                dbContext.Territories.AddRange(parsedTerritories);
                dbContext.SaveChanges();
            }
        }

        private static void SeedOrders(DataContext dbContext)
        {
            if (!dbContext.Orders.Any())
            {
                var ordersData = File.ReadAllText("./Resources/orders.json");
                var parsedOrders = JsonConvert.DeserializeObject<OrderDb[]>(ordersData);

                foreach (var order in parsedOrders)
                {
                    if (dbContext.Addresses.FirstOrDefault(a => a.Street == order.ShipAddress.Street) == null)
                    {
                        dbContext.Addresses.Add(order.ShipAddress);
                    }


                    dbContext.Orders.Add(order);
                }
                dbContext.SaveChanges();
            }
        }

        private static void SeedOrderDetails(DataContext dbContext)
        {
            if (!dbContext.OrderDetails.Any())
            {
                var ordersDetailsData = File.ReadAllText("./Resources/orderDetails.json");
                var parsedordersDetails = JsonConvert.DeserializeObject<OrderDetailDb[]>(ordersDetailsData);
                dbContext.OrderDetails.AddRange(parsedordersDetails);
                dbContext.SaveChanges();
            }
        }

        private static void SeedEmployees(DataContext dbContext)
        {
            if (!dbContext.Employees.Any())
            {
                var employeesData = File.ReadAllText("./Resources/employees.json");
                var parsedEmployees = JsonConvert.DeserializeObject<EmployeeDb[]>(employeesData);

                foreach (var employee in parsedEmployees)
                {
                    if (dbContext.Addresses.FirstOrDefault(a => a.Street == employee.Address.Street) == null)
                    {
                        dbContext.Addresses.Add(employee.Address);
                    }
                    dbContext.Employees.Add(employee);
                }
                dbContext.SaveChanges();
            }
        }

        private static void SeedCustomers(DataContext dbContext)
        {
            if (!dbContext.Customers.Any())
            {
                var customersData = File.ReadAllText("./Resources/customers.json");
                var parsedCustomers = JsonConvert.DeserializeObject<CustomerDb[]>(customersData);

                foreach (var customer in parsedCustomers)
                {
                    var existingCustomer = dbContext.Customers.Find(customer.CustomerId);

                    if (existingCustomer == null)
                    {
                        if (dbContext.Addresses.FirstOrDefault(a => a.Street == customer.Address.Street) == null)
                        {
                            dbContext.Addresses.Add(customer.Address);
                        }

                        dbContext.Customers.Add(customer);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        private static void SeedProducts(DataContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("./Resources/products.json");
                var parsedProducts = JsonConvert.DeserializeObject<ProductDb[]>(productsData);

                dbContext.Products.AddRange(parsedProducts);
                dbContext.SaveChanges();
            }
        }

        private static void SeedRegions(DataContext dbContext)
        {            
            if (!dbContext.Regions.Any())
            {
                var productsData = File.ReadAllText("./Resources/regions.json");
                var parsedRegions = JsonConvert.DeserializeObject<RegionDb[]>(productsData);


                foreach (var region in parsedRegions)
                {
                    var matchingTerritories = dbContext.Territories.Where(t => t.RegionId == region.RegionId).ToList();
                    region.Territories = matchingTerritories;

                    dbContext.Regions.Add(region);
                }
                dbContext.SaveChanges();
            }
        }

        private static void SeedCategories(DataContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                var categoriesData = File.ReadAllText("./Resources/categories.json");
                var parsedCategories = JsonConvert.DeserializeObject<CategoryDb[]>(categoriesData);

                dbContext.Categories.AddRange(parsedCategories);
                dbContext.SaveChanges();
            }
        }
    }
}
