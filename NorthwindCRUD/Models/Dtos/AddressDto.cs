namespace NorthwindCRUD.Models.Dtos
{
    using NorthwindCRUD.Models.Contracts;

    public class AddressDto : IAddress
    {
        public string Street {get; set;}

        public string City { get; set; }

        public string Region {get; set;}

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }
    }
}
