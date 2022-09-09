namespace NorthwindCRUD.Services
{
    using Microsoft.EntityFrameworkCore;
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

        public EmployeeDb GetById(int id)
        {
            return this.dataContext.Employees
                .Include(c => c.Address)
                .FirstOrDefault(c => c.EmployeeId == id);
        }

        public EmployeeDb Create(EmployeeDb model)
        {
            var employeeEntity = this.dataContext.Employees.Add(model);
            this.dataContext.SaveChanges();

            return employeeEntity.Entity;
        }

        public EmployeeDb Update(EmployeeDb model)
        {
            var employeeEntity = this.dataContext.Employees.FirstOrDefault(e => e.EmployeeId == model.EmployeeId);
            if (employeeEntity != null)
            {
                employeeEntity.LastName = model.LastName;
                employeeEntity.FirstName = model.FirstName;
                employeeEntity.Title = model.Title;
                employeeEntity.TitleOfCourtesy = model.TitleOfCourtesy;
                employeeEntity.BirthDate = model.BirthDate;
                employeeEntity.HireDate = model.HireDate;
                employeeEntity.Notes = model.Notes;
                employeeEntity.AvatarUrl = model.AvatarUrl;
                var newAddress = this.dataContext.Addresses.FirstOrDefault(a => a.Street == model.Address.Street);
                if (newAddress != null)
                {
                    employeeEntity.Address.City = model.Address.City;
                    employeeEntity.Address.Region = model.Address.Region;
                    employeeEntity.Address.PostalCode = model.Address.PostalCode;
                    employeeEntity.Address.Country = model.Address.Country;
                    employeeEntity.Address.Phone = model.Address.Phone;
                }
                else
                {
                    var employeeNewAddress = this.dataContext.Addresses.Add(model.Address);
                    employeeEntity.Address = employeeNewAddress.Entity;
                    employeeEntity.AddressId = employeeNewAddress.Entity.AddressId;
                }

                this.dataContext.SaveChanges();
            }

            return employeeEntity;
        }

        public EmployeeDb Delete(int id)
        {
            var employeeEntity = this.dataContext.Employees.FirstOrDefault(c => c.EmployeeId == id);
            if (employeeEntity != null)
            {
                this.dataContext.Employees.Remove(employeeEntity);
                this.dataContext.SaveChanges();
            }

            return employeeEntity;
        }
    }
}
