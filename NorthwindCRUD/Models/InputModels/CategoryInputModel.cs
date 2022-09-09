using NorthwindCRUD.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace NorthwindCRUD.Models.InputModels
{
    public class CategoryInputModel : ICategory
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
