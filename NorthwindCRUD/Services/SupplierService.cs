using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class SupplierService
    {
        private readonly DataContext dataContext;

        public SupplierService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public SupplierDb[] GetAll()
        {
            return this.dataContext.Suppliers.ToArray();
        }

        public IQueryable<SupplierDb> GetAllAsQueryable()
        {
            return this.dataContext.Suppliers;
        }

        public SupplierDb? GetById(int id)
        {
            return this.dataContext.Suppliers.FirstOrDefault(p => p.SupplierId == id);
        }

        public SupplierDb Create(SupplierDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }

            model.SupplierId = id;

            PropertyHelper<SupplierDb>.MakePropertiesEmptyIfNull(model);

            var supplierEntity = this.dataContext.Suppliers.Add(model);
            this.dataContext.SaveChanges();

            return supplierEntity.Entity;
        }

        public SupplierDb? Update(int id, SupplierDb model)
        {
            var supplierEntity = this.dataContext.Suppliers.FirstOrDefault(p => p.SupplierId == id);
            if (supplierEntity != null)
            {
                supplierEntity.Address = model.Address != null ? model.Address : supplierEntity.Address;
                supplierEntity.City = model.City != null ? model.City : supplierEntity.City;
                supplierEntity.Fax = model.Fax != null ? model.Fax : supplierEntity.Fax;
                supplierEntity.CompanyName = model.CompanyName != null ? model.CompanyName : supplierEntity.CompanyName;
                supplierEntity.ContactName = model.ContactName != null ? model.ContactName : supplierEntity.ContactName;
                supplierEntity.ContactTitle = model.ContactTitle != null ? model.ContactTitle : supplierEntity.ContactTitle;
                supplierEntity.Country = model.Country != null ? model.Country : supplierEntity.Country;
                supplierEntity.HomePage = model.HomePage != null ? model.HomePage : supplierEntity.HomePage;
                supplierEntity.Phone = model.Phone != null ? model.Phone : supplierEntity.Phone;
                supplierEntity.PostalCode = model.PostalCode != null ? model.PostalCode : supplierEntity.PostalCode;
                supplierEntity.Region = model.Region != null ? model.Region : supplierEntity.Region;

                this.dataContext.SaveChanges();
            }

            return supplierEntity;
        }

        public SupplierDb? Delete(int id)
        {
            var supplierEntity = this.GetById(id);
            if (supplierEntity != null)
            {
                this.dataContext.Suppliers.Remove(supplierEntity);
                this.dataContext.SaveChanges();
            }

            return supplierEntity;
        }
    }
}
