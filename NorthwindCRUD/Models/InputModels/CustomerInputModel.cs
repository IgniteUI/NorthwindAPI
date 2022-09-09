namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;
    using NorthwindCRUD.Models.DbModels;
    using System.ComponentModel.DataAnnotations;

    public class CustomerInputModel : ICustomer
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string ContactName { get; set; }

        [Required]
        public string ContactTitle { get; set; }

        [Required]
        public AddressInputModel Address { get; set; }
    }
}
