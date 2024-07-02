using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Helpers;
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

        public IQueryable<CustomerDb> GetAllAsQueryable()
        {
            return this.dataContext.Customers.AsQueryable();
        }

        public CustomerDb? GetById(string id)
        {
            return this.dataContext.Customers
                .Include(c => c.Address)
                .FirstOrDefault(c => c.CustomerId == id);
        }

        public CustomerDb Create(CustomerDb model)
        {
            var id = IdGenerator.CreateLetterId(6);
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateLetterId(6);
                existWithId = this.GetById(id);
            }

            model.CustomerId = id;

            PropertyHelper<CustomerDb>.MakePropertiesEmptyIfNull(model);

            if (model.Address == null)
            {
                var emptyAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == string.Empty);
                if (emptyAddress != null)
                {
                    model.Address = emptyAddress;
                    model.AddressId = emptyAddress.AddressId;
                }
            }

            var customerEntity = this.dataContext.Customers.Add(model);
            this.dataContext.SaveChanges();

            return customerEntity.Entity;
        }

        public CustomerDb? Update(CustomerDb model)
        {
            var customerEntity = this.dataContext.Customers
                .Include(c => c.Address)
                .FirstOrDefault(c => c.CustomerId == model.CustomerId);

            if (customerEntity != null)
            {
                customerEntity.CompanyName = model.CompanyName != null ? model.CompanyName : customerEntity.CompanyName;
                customerEntity.ContactName = model.ContactName != null ? model.ContactName : customerEntity.ContactName;
                customerEntity.ContactTitle = model.ContactTitle != null ? model.ContactTitle : customerEntity.ContactTitle;

                if (model.Address != null)
                {
                    var newAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == model.Address.Street);
                    if (newAddress != null)
                    {
                        customerEntity.Address.City = model.Address.City != null ? model.Address.City : customerEntity.Address.City;
                        customerEntity.Address.Region = model.Address.Region != null ? model.Address.Region : customerEntity.Address.Region;
                        customerEntity.Address.PostalCode = model.Address.PostalCode != null ? model.Address.PostalCode : customerEntity.Address.PostalCode;
                        customerEntity.Address.Country = model.Address.Country != null ? model.Address.Country : customerEntity.Address.Country;
                        customerEntity.Address.Phone = model.Address.Phone != null ? model.Address.Phone : customerEntity.Address.Phone;
                    }
                    else
                    {
                        var customerNewAddress = this.dataContext.Addresses.Add(model.Address);
                        customerEntity.Address = customerNewAddress.Entity;
                        customerEntity.AddressId = customerNewAddress.Entity.AddressId;
                    }
                }

                this.dataContext.SaveChanges();
            }

            return customerEntity;
        }

        public CustomerDb? Delete(string id)
        {
            var customerEntity = this.GetById(id);
            if (customerEntity != null)
            {
                this.dataContext.Customers.Remove(customerEntity);
                this.dataContext.SaveChanges();
            }

            return customerEntity;
        }
    }
}
