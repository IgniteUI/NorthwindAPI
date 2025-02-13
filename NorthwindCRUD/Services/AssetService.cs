namespace NorthwindCRUD.Services
{
    using NorthwindCRUD.Models.DbModels;

    public class AssetService
    {
        private readonly DataContext dataContext;

        public AssetService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public AssetDb[] GetAll()
        {
            return this.dataContext.Assets.ToArray();
        }
    }
}
