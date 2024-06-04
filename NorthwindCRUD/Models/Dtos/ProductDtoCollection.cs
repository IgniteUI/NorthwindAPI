using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Dtos
{
    public class PagedProductsDto
    {
        //public List<ProductDto> Products { get; set; }
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();

        public int TotalRecordsCount { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }
    }
}
