using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class CustomerService
    {
        private readonly DataContext dataContext;

        public CustomerService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public CustomerDb[] GetAll()
        {
            return this.dataContext.Customers
                .Include(c => c.Address)
                .ToArray();
        }

        public CustomerDb GetById(string id)
        {
            return this.dataContext.Customers
                .Include(c => c.Address)
                .FirstOrDefault(c => c.CustomerId == id);
        }

        public CustomerDb Create(CustomerDb model)
        {
            var customerEntity = this.dataContext.Customers.Add(model);
            this.dataContext.SaveChanges();
            
            return customerEntity.Entity;
        }

        public CustomerDb Update(CustomerDb model)
        {
            var customerEntity = this.dataContext.Customers.FirstOrDefault(c => c.CustomerId == model.CustomerId);
            if (customerEntity != null)
            {
                customerEntity.CompanyName = model.CompanyName;
                customerEntity.ContactName = model.ContactName;
                customerEntity.ContactTitle = model.ContactTitle;
                var newAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == model.Address.Street);
                if (newAddress != null)
                {
                    customerEntity.Address.City = model.Address.City;
                    customerEntity.Address.Region = model.Address.Region;
                    customerEntity.Address.PostalCode = model.Address.PostalCode;
                    customerEntity.Address.Country = model.Address.Country;
                    customerEntity.Address.Phone = model.Address.Phone;
                }
                else
                {
                    var customerNewAddress = this.dataContext.Addresses.Add(model.Address);
                    customerEntity.Address = customerNewAddress.Entity;
                    customerEntity.AddressId = customerNewAddress.Entity.AddressId;
                }

                this.dataContext.SaveChanges();
            }

            return customerEntity;
        }

        public CustomerDb Delete(string id)
        {
            var customerEntity = this.dataContext.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (customerEntity != null)
            {
                this.dataContext.Customers.Remove(customerEntity);
                this.dataContext.SaveChanges();
            }

            return customerEntity;
        }
    }
}
