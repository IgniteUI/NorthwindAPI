using AutoMapper;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public class ProductService : BaseDbService<ProductDto, ProductDb, int>
    {
        public ProductService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
        }

        public ProductDto[] GetAllByCategoryId(int id)
        {
            var products = this.GetAll().Where(p => p.CategoryId == id).ToArray();
            return products;
        }

        public ProductDto[] GetAllBySupplierId(int id)
        {
            var products = this.GetAll().Where(p => p.SupplierId == id).ToArray();
            return products;
        }

        public ProductDto[] GetProductsByIds(int[] productIds)
        {
            var products = this.GetAll().Where(p => productIds.Contains(p.ProductId)).ToArray();
            return products;
        }
    }
}
