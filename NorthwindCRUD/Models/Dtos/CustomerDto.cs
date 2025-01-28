using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class CustomerDto : IBaseDto, ICustomer
    {
        [Key]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }

        [StringLength(50, ErrorMessage = "Contact Name cannot exceed 50 characters.")]
        public string ContactName { get; set; }

        [StringLength(50, ErrorMessage = "Contact Title cannot exceed 50 characters.")]
        public string ContactTitle { get; set; }

        public AddressDto Address { get; set; }
    }
}
