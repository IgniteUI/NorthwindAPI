using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class SupplierDto : IBaseDto, ISupplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string? CompanyName { get; set; }

        [StringLength(100, ErrorMessage = "Contact Name cannot exceed 100 characters.")]
        public string? ContactName { get; set; }

        [StringLength(50, ErrorMessage = "Contact Title cannot exceed 50 characters.")]
        public string? ContactTitle { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        public string? Region { get; set; }

        [StringLength(20, ErrorMessage = "Postal Code cannot exceed 20 characters.")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string? Country { get; set; }

        [RegularExpression(@"^\+?\(?\d{1,5}\)?[-.\s]?\(?\d{1,5}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,10}$", ErrorMessage = "Phone number is not valid.")]
        public string? Phone { get; set; }

        [RegularExpression(@"^\+?\(?\d{1,5}\)?[-.\s]?\(?\d{1,5}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,10}$", ErrorMessage = "Fax number is not valid.")]
        public string? Fax { get; set; }

        [RegularExpression(@"^https?:\/\/[^\s$.?#].[^\s]*$", ErrorMessage = "Home Page URL is not valid.")]
        public string? HomePage { get; set; }
    }
}
