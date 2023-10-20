using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class ShipperService
    {
        private readonly DataContext dataContext;

        public ShipperService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public ShipperDb[] GetAll()
        {
            return this.dataContext.Shippers.ToArray();
        }

        public ShipperDb GetById(int id)
        {
            return this.dataContext.Shippers.FirstOrDefault(p => p.ShipperId == id);
        }

        public ShipperDb Create(ShipperDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }
            model.ShipperId = id;

            PropertyHelper<ShipperDb>.MakePropertiesEmptyIfNull(model);

            var shipperEntity = this.dataContext.Shippers.Add(model);
            this.dataContext.SaveChanges();

            return shipperEntity.Entity;
        }

        public ShipperDb Update(ShipperDb model)
        {
            var shipperEntity = this.dataContext.Shippers.FirstOrDefault(p => p.ShipperId == model.ShipperId);
            if (shipperEntity != null)
            {
                shipperEntity.Phone = model.Phone != null ? model.Phone : shipperEntity.Phone;
                shipperEntity.CompanyName= model.CompanyName != null ? model.CompanyName : shipperEntity.CompanyName;

                this.dataContext.SaveChanges();
            }

            return shipperEntity;
        }

        public ShipperDb Delete(int id)
        {
            var shipperEntity = this.GetById(id);
            if (shipperEntity != null)
            {
                this.dataContext.Shippers.Remove(shipperEntity);
                this.dataContext.SaveChanges();
            }

            return shipperEntity;
        }
    }
}
