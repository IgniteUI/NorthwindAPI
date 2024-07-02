using System.Globalization;
using NorthwindCRUD.Constants;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class TerritoryService
    {
        private readonly DataContext dataContext;

        public TerritoryService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public TerritoryDb[] GetAll()
        {
            return this.dataContext.Territories.ToArray();
        }

        public IQueryable<TerritoryDb> GetAllAsQueryable()
        {
            return this.dataContext.Territories.AsQueryable();
        }

        public TerritoryDb? GetById(string id)
        {
            return this.dataContext.Territories.FirstOrDefault(t => t.TerritoryId == id);
        }

        public TerritoryDb[] GetTerritoriesByRegionId(int id)
        {
            return this.dataContext.Territories.Where(t => t.RegionId == id).ToArray();
        }

        public TerritoryDb Create(TerritoryDb model)
        {
            if (this.dataContext.Regions.FirstOrDefault(r => r.RegionId == model.RegionId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Region), model.RegionId?.ToString(CultureInfo.InvariantCulture)));
            }

            var id = IdGenerator.CreateDigitsId().ToString(CultureInfo.InvariantCulture);
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId().ToString(CultureInfo.InvariantCulture);
                existWithId = this.GetById(id);
            }

            model.TerritoryId = id;

            PropertyHelper<TerritoryDb>.MakePropertiesEmptyIfNull(model);

            var territoryEntity = this.dataContext.Territories.Add(model);
            this.dataContext.SaveChanges();

            return territoryEntity.Entity;
        }

        public TerritoryDb? Update(TerritoryDb model)
        {
            if (this.dataContext.Regions.FirstOrDefault(r => r.RegionId == model.RegionId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Region), model.RegionId?.ToString(CultureInfo.InvariantCulture)));
            }

            var territoryEntity = this.dataContext.Territories.FirstOrDefault(p => p.TerritoryId == model.TerritoryId);
            if (territoryEntity != null)
            {
                territoryEntity.TerritoryDescription = model.TerritoryDescription != null ? model.TerritoryDescription : territoryEntity.TerritoryDescription;
                territoryEntity.RegionId = model.RegionId != null ? model.RegionId : territoryEntity.RegionId;

                this.dataContext.SaveChanges();
            }

            return territoryEntity;
        }

        public TerritoryDb? Delete(string id)
        {
            var territoryEntity = this.GetById(id);
            if (territoryEntity != null)
            {
                this.dataContext.Territories.Remove(territoryEntity);
                this.dataContext.SaveChanges();
            }

            return territoryEntity;
        }
    }
}
