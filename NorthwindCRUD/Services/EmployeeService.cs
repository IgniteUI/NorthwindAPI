namespace NorthwindCRUD.Services
{
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Helpers;
    using NorthwindCRUD.Models.DbModels;

    public class EmployeeService
    {
        private readonly DataContext dataContext;

        public EmployeeService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public EmployeeDb[] GetAll()
        {
            return this.dataContext.Employees
                .Include(c => c.Address)
                .ToArray();
        }

        public IQueryable<EmployeeDb> GetAllAsQueryable()
        {
            return this.dataContext.Employees
                .Include(c => c.Address);
        }

        public EmployeeDb? GetById(int id)
        {
            return this.dataContext.Employees
                .Include(x => x.Address)
                .FirstOrDefault(c => c.EmployeeId == id);
        }

        public EmployeeDb[] GetEmployeesByReportsTo(int id)
        {
            return this.dataContext.Employees
                .Where(c => c.ReportsTo == id)
                .ToArray();
        }

        public EmployeeDb Create(EmployeeDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }

            model.EmployeeId = id;

            PropertyHelper<EmployeeDb>.MakePropertiesEmptyIfNull(model);

            if (model.Address == null)
            {
                var emptyAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == string.Empty);
                if (emptyAddress != null)
                {
                    model.Address = emptyAddress;
                    model.AddressId = emptyAddress.AddressId;
                }
            }

            var employeeEntity = this.dataContext.Employees.Add(model);
            this.dataContext.SaveChanges();

            return employeeEntity.Entity;
        }

        public EmployeeDb? Update(int id, EmployeeDb model)
        {
            var employeeEntity = this.dataContext.Employees
                .Include(c => c.Address)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employeeEntity != null)
            {
                employeeEntity.LastName = model.LastName != null ? model.LastName : employeeEntity.LastName;
                employeeEntity.FirstName = model.FirstName != null ? model.FirstName : employeeEntity.FirstName;
                employeeEntity.Title = model.Title != null ? model.Title : employeeEntity.Title;
                employeeEntity.TitleOfCourtesy = model.TitleOfCourtesy != null ? model.TitleOfCourtesy : employeeEntity.TitleOfCourtesy;
                employeeEntity.BirthDate = model.BirthDate != null ? model.BirthDate : employeeEntity.BirthDate;
                employeeEntity.HireDate = model.HireDate != null ? model.HireDate : employeeEntity.HireDate;
                employeeEntity.Notes = model.Notes != null ? model.Notes : employeeEntity.Notes;
                employeeEntity.AvatarUrl = model.AvatarUrl != null ? model.AvatarUrl : employeeEntity.AvatarUrl;

                if (model.Address != null)
                {
                    var newAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == model.Address.Street);
                    if (newAddress != null)
                    {
                        employeeEntity.Address.City = model.Address.City != null ? model.Address.City : employeeEntity.Address.City;
                        employeeEntity.Address.Region = model.Address.Region != null ? model.Address.Region : employeeEntity.Address.Region;
                        employeeEntity.Address.PostalCode = model.Address.PostalCode != null ? model.Address.PostalCode : employeeEntity.Address.PostalCode;
                        employeeEntity.Address.Country = model.Address.Country != null ? model.Address.Country : employeeEntity.Address.Country;
                        employeeEntity.Address.Phone = model.Address.Phone != null ? model.Address.Phone : employeeEntity.Address.Phone;
                    }
                    else
                    {
                        var employeeNewAddress = this.dataContext.Addresses.Add(model.Address);
                        employeeEntity.Address = employeeNewAddress.Entity;
                        employeeEntity.AddressId = employeeNewAddress.Entity.AddressId;
                    }
                }

                this.dataContext.SaveChanges();
            }

            return employeeEntity;
        }

        public EmployeeDb? Delete(int id)
        {
            var employeeEntity = this.GetById(id);
            if (employeeEntity != null)
            {
                this.dataContext.Employees.Remove(employeeEntity);
                this.dataContext.SaveChanges();
            }

            return employeeEntity;
        }
    }
}
