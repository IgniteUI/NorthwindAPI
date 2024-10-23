using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class ShipperDto : IBaseDto, IShipper
    {
        [Key]
        public int ShipperId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }

        [RegularExpression(@"^\+?\(?\d{1,5}\)?[-.\s]?\(?\d{1,5}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,10}$", ErrorMessage = "Phone number is not valid.")]
        public string Phone { get; set; }
    }
}
