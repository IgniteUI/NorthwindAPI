namespace NorthwindCRUD.Services
{
    using AutoMapper;
    using NorthwindCRUD.Helpers;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;

    public class ProductService
    {

        private readonly IMapper mapper;
        private readonly DataContext dataContext;

        public ProductService(IMapper mapper, DataContext dataContext)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        public ProductDb[] GetAll()
        {
            return this.dataContext.Products.ToArray();
        }

        public ProductDb GetById(int id)
        {
            return this.dataContext.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public ProductDb Create(ProductDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }
            model.ProductId = id;

            PropertyHelper<ProductDb>.MakePropertiesEmptyIfNull(model);

            var productEntity = this.dataContext.Products.Add(model);
            this.dataContext.SaveChanges();

            return productEntity.Entity;
        }

        public ProductDb Update(ProductDb model)
        {
            var productEntity = this.dataContext.Products.FirstOrDefault(p => p.ProductId == model.ProductId);
            if (productEntity != null)
            {
                productEntity.SupplierId = model.SupplierId != null ? model.SupplierId : productEntity.SupplierId;
                productEntity.CategoryId = model.CategoryId != null ? model.CategoryId : productEntity.CategoryId;
                productEntity.QuantityPerUnit = model.QuantityPerUnit != null ? model.QuantityPerUnit : productEntity.QuantityPerUnit;
                productEntity.UnitPrice = model.UnitPrice != null ? model.UnitPrice : productEntity.UnitPrice;
                productEntity.UnitsInStock = model.UnitsInStock != null ? model.UnitsInStock : productEntity.UnitsInStock;
                productEntity.UnitsOnOrder = model.UnitsOnOrder != null ? model.UnitsOnOrder : productEntity.UnitsOnOrder;
                productEntity.ReorderLevel = model.ReorderLevel != null ? model.ReorderLevel : productEntity.ReorderLevel;
                productEntity.Discontinued = model.Discontinued != productEntity.Discontinued ? model.Discontinued : productEntity.Discontinued;

                this.dataContext.SaveChanges();
            }

            return productEntity;
        }

        public ProductDb Delete(int id)
        {
            var productEntity = this.GetById(id);
            if (productEntity != null)
            {
                this.dataContext.Products.Remove(productEntity);
                this.dataContext.SaveChanges();
            }

            return productEntity;
        }
    }
}
