namespace NorthwindCRUD.Helpers
{
    using Microsoft.AspNetCore.JsonPatch.Internal;
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
                // Seed Categories
                if (!dbContext.Categories.Any())
                {
                    var categoriesData = File.ReadAllText("./Resources/categories.json");
                    var parsedCategories = JsonConvert.DeserializeObject<CategoryDb[]>(categoriesData);

                    dbContext.Categories.AddRange(parsedCategories);
                    dbContext.SaveChanges();
                }

                // Seed Customers
                if (!dbContext.Customers.Any())
                {
                    var customersData = File.ReadAllText("./Resources/customers.json");
                    var parsedCustomers = JsonConvert.DeserializeObject<CustomerDb[]>(customersData);

                    foreach (var customer in parsedCustomers)
                    {
                        if (dbContext.Addresses.FirstOrDefault(a => a.Street == customer.Address.Street) == null)
                        {
                            dbContext.Addresses.Add(customer.Address);
                        }
                        dbContext.Customers.Add(customer);
                    }
                    dbContext.SaveChanges();
                }

                // Seed Employees
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

                // Seed Orders
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

                        if (!dbContext.OrderDetails.Any(o => o.OrderId == order.OrderId))
                        {
                            var orderDetailsData = order.Details.ToList();

                            orderDetailsData.ForEach(o => 
                            { 
                               o.OrderId = order.OrderId;
                            });

                            dbContext.OrderDetails.AddRange(order.Details);
                        }

                        dbContext.Orders.Add(order);
                    }
                    dbContext.SaveChanges();
                }

                //Seed Products
                if (!dbContext.Products.Any())
                {
                    var productsData = File.ReadAllText("./Resources/products.json");
                    var parsedProducts = JsonConvert.DeserializeObject<ProductDb[]>(productsData);

                    dbContext.Products.AddRange(parsedProducts);
                    dbContext.SaveChanges();
                }

                //Seed Regions
                if (!dbContext.Regions.Any())
                {
                    var productsData = File.ReadAllText("./Resources/regions.json");
                    var parsedRegions = JsonConvert.DeserializeObject<RegionDb[]>(productsData);

                    dbContext.Regions.AddRange(parsedRegions);
                    dbContext.SaveChanges();
                }

                //Seed Shippers
                if (!dbContext.Shippers.Any())
                {
                    var shippersData = File.ReadAllText("./Resources/shippers.json");
                    var parsedShippers = JsonConvert.DeserializeObject<ShipperDb[]>(shippersData);

                    dbContext.Shippers.AddRange(parsedShippers);
                    dbContext.SaveChanges();
                }

                //Seed Suppliers
                if (!dbContext.Suppliers.Any())
                {
                    var suppliersData = File.ReadAllText("./Resources/suppliers.json");
                    var parsedSuppliers = JsonConvert.DeserializeObject<SupplierDb[]>(suppliersData);

                    dbContext.Suppliers.AddRange(parsedSuppliers);
                    dbContext.SaveChanges();
                }

                //Seed Territories
                if (!dbContext.Territories.Any())
                {
                    var territoriesData = File.ReadAllText("./Resources/territories.json");
                    var parsedTerritories = JsonConvert.DeserializeObject<TerritoryDb[]>(territoriesData);

                    dbContext.Territories.AddRange(parsedTerritories);
                    dbContext.SaveChanges();
                }

                //Seed EmployeesTeritories
                if (!dbContext.EmployeesTerritories.Any())
                {
                    var employeesTerritoriesData = File.ReadAllText("./Resources/employees-territories.json");
                    var parsedEmployeesTerritories = JsonConvert.DeserializeObject<EmployeeTerritoryDb[]>(employeesTerritoriesData);

                    dbContext.EmployeesTerritories.AddRange(parsedEmployeesTerritories);
                    dbContext.SaveChanges();
                }

                transaction.Commit();
            }
            catch (Exception error)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
