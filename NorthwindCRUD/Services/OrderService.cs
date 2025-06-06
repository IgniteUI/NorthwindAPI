using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Constants;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class OrderService
    {
        private readonly DataContext dataContext;

        public OrderService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public OrderDb[] GetAll()
        {
            return this.dataContext.Orders
                .Include(c => c.ShipAddress)
                .ToArray();
        }

        public IQueryable<OrderDb> GetAllAsQueryable()
        {
            return this.dataContext.Orders
                .Include(c => c.ShipAddress);
        }

        public OrderDb[] GetNOrders(int numberOfOrdersToRetrieve)
        {
            return this.dataContext.Orders
                .Include(c => c.ShipAddress)
                .Take(numberOfOrdersToRetrieve)
                .ToArray();
        }

        public OrderDb? GetById(int id)
        {
            return GetOrdersQuery().FirstOrDefault(c => c.OrderId == id);
        }

        public OrderDetailDb[] GetOrderDetailsById(int id)
        {
            var details = this.dataContext.OrderDetails.Where(o => o.OrderId == id).ToArray();
            return details;
        }

        public OrderDb[] GetOrdersByCustomerId(string id)
        {
            return GetOrdersQuery()
                .Where(o => o.CustomerId == id)
                .ToArray();
        }

        public OrderDb[] GetOrdersWithDetailsByCustomerId(string id)
        {
            return GetOrdersWithDetailsQuery()
                .Where(o => o.CustomerId == id)
                .AsNoTracking()
                .ToArray();
        }

        public OrderDb[] GetOrdersByEmployeeId(int id)
        {
            return GetOrdersQuery()
                .Where(o => o.EmployeeId == id)
                .ToArray();
        }

        public OrderDb[] GetOrdersByShipperId(int id)
        {
            return GetOrdersQuery()
                .Where(o => o.ShipperId == id)
                .ToArray();
        }

        public OrderDetailDb[] GetOrderDetailsByProductId(int id)
        {
            var details = this.dataContext.OrderDetails
                .Where(o => o.ProductId == id)
                .ToArray();

            return details;
        }

        public OrderDb Create(OrderDb model)
        {
            if (this.dataContext.Customers.FirstOrDefault(c => c.CustomerId == model.CustomerId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Customer), model.CustomerId?.ToString(CultureInfo.InvariantCulture)));
            }

            if (this.dataContext.Employees.FirstOrDefault(e => e.EmployeeId == model.EmployeeId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Employee), model.EmployeeId?.ToString(CultureInfo.InvariantCulture)));
            }

            if (this.dataContext.Shippers.FirstOrDefault(s => s.ShipperId == model.ShipperId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Shipper), model.ShipperId?.ToString(CultureInfo.InvariantCulture)));
            }

            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }

            model.OrderId = id;

            PropertyHelper<OrderDb>.MakePropertiesEmptyIfNull(model);

            if (model.ShipAddress == null)
            {
                var emptyAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == string.Empty);
                model.ShipAddress = emptyAddress;
                model.ShipAddressId = emptyAddress?.AddressId;
            }

            var orderEntity = this.dataContext.Orders.Add(model);
            this.dataContext.SaveChanges();

            return orderEntity.Entity;
        }

        public OrderDb? Update(int id, OrderDb model)
        {
            if (this.dataContext.Customers.FirstOrDefault(c => c.CustomerId == model.CustomerId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Customer), model.CustomerId?.ToString()));
            }

            if (this.dataContext.Employees.FirstOrDefault(e => e.EmployeeId == model.EmployeeId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Employee), model.EmployeeId?.ToString(CultureInfo.InvariantCulture)));
            }

            if (this.dataContext.Shippers.FirstOrDefault(s => s.ShipperId == model.ShipperId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Shipper), model.ShipperId?.ToString(CultureInfo.InvariantCulture)));
            }

            var orderEntity = this.dataContext.Orders
                .Include(c => c.ShipAddress)
                .FirstOrDefault(e => e.OrderId == id);

            if (orderEntity != null)
            {
                orderEntity.OrderDate = model.OrderDate != null ? model.OrderDate : orderEntity.OrderDate;
                orderEntity.RequiredDate = model.RequiredDate != null ? model.RequiredDate : orderEntity.RequiredDate;
                orderEntity.ShipVia = model.ShipVia; // ShipVia has int type which can't be null
                orderEntity.Freight = model.Freight; // Freight has double type which can't be null
                orderEntity.ShipName = model.ShipName != null ? model.ShipName : orderEntity.ShipName;
                orderEntity.EmployeeId = model.EmployeeId != null ? model.EmployeeId : orderEntity.EmployeeId;
                orderEntity.CustomerId = model.CustomerId != null ? model.CustomerId : orderEntity.CustomerId;

                if (model.ShipAddress != null)
                {
                    var newAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == model.ShipAddress.Street);
                    if (newAddress != null && orderEntity.ShipAddress != null)
                    {
                        orderEntity.ShipAddress.City = model.ShipAddress.City;
                        orderEntity.ShipAddress.Region = model.ShipAddress.Region;
                        orderEntity.ShipAddress.PostalCode = model.ShipAddress.PostalCode;
                        orderEntity.ShipAddress.Country = model.ShipAddress.Country;
                        orderEntity.ShipAddress.Phone = model.ShipAddress.Phone;
                    }
                    else
                    {
                        var employeeNewAddress = this.dataContext.Addresses.Add(model.ShipAddress);
                        orderEntity.ShipAddress = employeeNewAddress.Entity;
                        orderEntity.ShipAddressId = employeeNewAddress.Entity.AddressId;
                    }
                }

                this.dataContext.SaveChanges();
            }

            return orderEntity;
        }

        public OrderDb? Delete(int id)
        {
            var orderEntity = this.GetById(id);
            if (orderEntity != null)
            {
                this.dataContext.Orders.Remove(orderEntity);
                this.dataContext.SaveChanges();
            }

            return orderEntity;
        }

        private IQueryable<OrderDb> GetOrdersQuery()
        {
            return this.dataContext.Orders
                .Include(c => c.ShipAddress);
        }

        private IQueryable<OrderDb> GetOrdersWithDetailsQuery()
        {
            return this.dataContext.Orders
                .Include(c => c.OrderDetails)
                .Include(c => c.ShipAddress);
        }
    }
}
