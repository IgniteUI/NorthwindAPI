using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

    public class AddressDto : IBaseDto, IAddress
    {
        [StringLength(100, ErrorMessage = "Street cannot exceed 100 characters.")]
        public string Street { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "Region cannot exceed 50 characters.")]
        public string Region { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string Country { get; set; }

        [RegularExpression(@"^\+?\(?\d{1,5}\)?[-.\s]?\(?\d{1,5}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,10}$", ErrorMessage = "Phone number is not valid.")]
        public string? Phone { get; set; }
    }
}
