namespace NorthwindCRUD.Services
{
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Models.DbModels;

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

        public OrderDb GetById(int id)
        {
            return this.dataContext.Orders
                .Include(c => c.ShipAddress)
                .FirstOrDefault(c => c.OrderId == id);
        }

        public OrderDb Create(OrderDb model)
        {
            var orderEntity = this.dataContext.Orders.Add(model);
            this.dataContext.SaveChanges();

            return orderEntity.Entity;
        }
        
        public OrderDb Update(OrderDb model)
        {
            var orderEntity = this.dataContext.Orders.FirstOrDefault(e => e.OrderId == model.OrderId);
            if (orderEntity != null)
            {
                orderEntity.OrderDate = model.OrderDate;
                orderEntity.RequiredDate = model.RequiredDate;
                orderEntity.ShipVia = model.ShipVia;
                orderEntity.Freight = model.Freight;
                orderEntity.ShipName = model.ShipName;
                orderEntity.EmployeeId = model.EmployeeId;
                orderEntity.CustomerId = model.CustomerId;
                
                var newAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == model.ShipAddress.Street);
                if (newAddress != null)
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

                this.dataContext.SaveChanges();
            }

            return orderEntity;
        }

        public OrderDb Delete(int id)
        {
            var orderEntity = this.dataContext.Orders.FirstOrDefault(c => c.EmployeeId == id);
            if (orderEntity != null)
            {
                this.dataContext.Orders.Remove(orderEntity);
                this.dataContext.SaveChanges();
            }

            return orderEntity;
        }
    }
}
