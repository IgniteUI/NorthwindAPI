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
                        dbContext.Orders.Add(order);
                    }
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
