using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class CustomerDto : IBaseDto, ICustomer
    {
        public string CustomerId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public AddressDto Address { get; set; }
    }
}
