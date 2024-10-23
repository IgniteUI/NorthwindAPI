using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class CategoryDto : IBaseDto, ICategory
    {
        [Key]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
