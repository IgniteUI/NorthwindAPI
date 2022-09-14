namespace NorthwindCRUD.Services
{
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Helpers;
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
                var emptyAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == "");
                model.ShipAddress = emptyAddress;
                model.ShipAddressId = emptyAddress.AddressId;
            }

            var orderEntity = this.dataContext.Orders.Add(model);
            this.dataContext.SaveChanges();

            return orderEntity.Entity;
        }
        
        public OrderDb Update(OrderDb model)
        {
            var orderEntity = this.dataContext.Orders
                .Include(c => c.ShipAddress)
                .FirstOrDefault(e => e.OrderId == model.OrderId);

            if (orderEntity != null)
            {
                orderEntity.OrderDate = model.OrderDate != null ? model.OrderDate : orderEntity.OrderDate;
                orderEntity.RequiredDate = model.RequiredDate != null ? model.RequiredDate : orderEntity.RequiredDate;
                orderEntity.ShipVia = model.ShipVia != null ? model.ShipVia : orderEntity.ShipVia;
                orderEntity.Freight = model.Freight != null ? model.Freight : orderEntity.Freight;
                orderEntity.ShipName = model.ShipName != null ? model.ShipName : orderEntity.ShipName;
                orderEntity.EmployeeId = model.EmployeeId != null ? model.EmployeeId : orderEntity.EmployeeId;
                orderEntity.CustomerId = model.CustomerId != null ? model.CustomerId : orderEntity.CustomerId;

                if (model.ShipAddress != null)
                {
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
                }

                this.dataContext.SaveChanges();
            }

            return orderEntity;
        }

        public OrderDb Delete(int id)
        {
            var orderEntity = this.GetById(id);
            if (orderEntity != null)
            {
                this.dataContext.Orders.Remove(orderEntity);
                this.dataContext.SaveChanges();
            }

            return orderEntity;
        }
    }
}
