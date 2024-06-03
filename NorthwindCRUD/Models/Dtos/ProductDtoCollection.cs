using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Dtos
{
    public class ProductDtoCollection
    {
        //public List<ProductDto> Products { get; set; }
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();

        public int TotalRecordsCount { get; set; }
    }
}
