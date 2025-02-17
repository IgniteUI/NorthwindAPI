namespace NorthwindCRUD.Services
{
    using Microsoft.EntityFrameworkCore;
    using NorthwindCRUD.Models.DbModels;

    public class VehicleService
    {
        private readonly DataContext dataContext;

        public VehicleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public VehicleDb[] GetAll()
        {
            return this.dataContext.Vehicles.Include(v => v.Details).ToArray();
        }
    }
}
