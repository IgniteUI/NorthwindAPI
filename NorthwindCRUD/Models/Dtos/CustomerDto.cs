namespace NorthwindCRUD.Models.Dtos
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class CustomerDto : ICustomer
    {
        public string CustomerId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public AddressDto Address { get; set; }
    }
}
