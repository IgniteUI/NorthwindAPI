using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class AddressDto : IAddress
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        public string? Phone { get; set; }
    }
}
