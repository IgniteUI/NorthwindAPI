﻿using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class RegionService
    {

        private readonly DataContext dataContext;

        public RegionService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public RegionDb[] GetAll()
        {
            return this.dataContext.Regions.ToArray();
        }

        public RegionDb GetById(int id)
        {
            return this.dataContext.Regions.FirstOrDefault(p => p.RegionId == id);
        }

        public RegionDb Create(RegionDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }
            model.RegionId = id;

            PropertyHelper<RegionDb>.MakePropertiesEmptyIfNull(model);

            var RegionEntity = this.dataContext.Regions.Add(model);
            this.dataContext.SaveChanges();

            return RegionEntity.Entity;
        }

        public RegionDb Update(RegionDb model)
        {
            var regionEntity = this.dataContext.Regions.FirstOrDefault(p => p.RegionId == model.RegionId);
            if (regionEntity != null)
            {
                regionEntity.RegionDescription = model.RegionDescription != null ? model.RegionDescription : regionEntity.RegionDescription;

                this.dataContext.SaveChanges();
            }

            return regionEntity;
        }

        public RegionDb Delete(int id)
        {
            var RegionEntity = this.GetById(id);
            if (RegionEntity != null)
            {
                this.dataContext.Regions.Remove(RegionEntity);
                this.dataContext.SaveChanges();
            }

            return RegionEntity;
        }
    }
}
