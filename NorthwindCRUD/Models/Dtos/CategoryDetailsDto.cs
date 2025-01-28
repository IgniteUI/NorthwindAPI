using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class CategoryDetailsDto : CategoryDto, IBaseDto, ICategoryDetail
    {
        [RegularExpression(@"^(http[s]?://.*\.(?:jpg|jpeg|png|gif))$", ErrorMessage = "Picture URL must start with http/s and end with .jpg, .jpeg, .png, or .gif.")]
        public string Picture { get; set; }
    }
}
