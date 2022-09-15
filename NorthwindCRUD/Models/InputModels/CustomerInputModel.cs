namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class CustomerInputModel : ICustomer
    {
        public string CustomerId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public AddressInputModel Address { get; set; }
    }
}
