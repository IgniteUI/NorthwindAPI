namespace NorthwindCRUD.Services
{
    using NorthwindCRUD.Models.DbModels;

    public class BrandService
    {
        private readonly DataContext dataContext;

        public BrandService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public BrandSaleDb[] GetAll()
        {
            return this.dataContext.BrandSales.ToArray();
        }
    }
}
