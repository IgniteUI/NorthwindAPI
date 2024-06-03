using System.Globalization;
using NorthwindCRUD.Constants;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Services
{
    public class ProductService
    {
        private readonly DataContext dataContext;

        public ProductService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public ProductDb[] GetAll()
        {
            return this.dataContext.Products.ToArray();
        }

        public ProductDb? GetById(int id)
        {
            return this.dataContext.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public ProductDb[] GetAllByCategoryId(int id)
        {
            return this.dataContext.Products.Where(p => p.CategoryId == id).ToArray();
        }

        // New method for paging with category filter
        public ProductDb[] GetPagedProductsByCategoryId(int categoryId, int pageNumber, int pageSize)
        {
            return this.dataContext.Products
                                   .Where(p => p.CategoryId == categoryId)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToArray();
        }

        // New method for paging with supplier filter
        public ProductDb[] GetPagedProductsBySupplierId(int supplierId, int pageNumber, int pageSize)
        {
            return this.dataContext.Products
                                   .Where(p => p.SupplierId == supplierId)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToArray();
        }

        public ProductDb[] GetAllBySupplierId(int id)
        {
            return this.dataContext.Products.Where(p => p.SupplierId == id).ToArray();
        }

        public ProductDb[] GetProductsByIds(int[] productIds)
        {
            return this.dataContext.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToArray();
        }

        public ProductDb Create(ProductDb model)
        {
            if (this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Category), model.CategoryId?.ToString(CultureInfo.InvariantCulture)));
            }

            if (this.dataContext.Suppliers.FirstOrDefault(s => s.SupplierId == model.SupplierId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Supplier), model.SupplierId?.ToString(CultureInfo.InvariantCulture)));
            }

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

        public ProductDb? Update(ProductDb model)
        {
            if (this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Category), model.CategoryId?.ToString(CultureInfo.InvariantCulture)));
            }

            if (this.dataContext.Suppliers.FirstOrDefault(s => s.SupplierId == model.SupplierId) == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, StringTemplates.InvalidEntityMessage, nameof(model.Supplier), model.SupplierId?.ToString(CultureInfo.InvariantCulture)));
            }

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

        public ProductDb? Delete(int id)
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
