using NorthwindCRUD.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace NorthwindCRUD.Models.InputModels
{
    public class CategoryInputModel : ICategory
    {
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
