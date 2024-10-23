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
            var products = this.dataContext.Products.Where(p => p.CategoryId == id).ToArray();
            return mapper.Map<ProductDto[]>(products);
        }

        public ProductDto[] GetAllBySupplierId(int id)
        {
            var products = this.dataContext.Products.Where(p => p.SupplierId == id).ToArray();
            return mapper.Map<ProductDto[]>(products);
        }

        public ProductDto[] GetProductsByIds(int[] productIds)
        {
            var products = this.dataContext.Products.Where(p => productIds.Contains(p.ProductId)).ToArray();
            return mapper.Map<ProductDto[]>(products);
        }
    }
}
