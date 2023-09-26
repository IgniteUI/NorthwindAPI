using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class CategoryDetailsDto : CategoryDto, ICategoryDetail
    {
        public string Picture { get; set; }
    }
}
