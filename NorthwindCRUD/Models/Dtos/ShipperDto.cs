using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class ShipperDto : IShipper
    {
        public int ShipperId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number is not valid.")]
        public string Phone { get; set; }
    }
}
