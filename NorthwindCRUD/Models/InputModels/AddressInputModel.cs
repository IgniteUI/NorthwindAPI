namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;

    public class AddressInputModel : IAddress
    {
        public string Street {get; set;}
        public string City { get; set; }
        public string Region {get; set;}
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
