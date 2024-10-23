using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class AddressDb : IBaseDb, IAddress
    {

        public string Street { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string? Phone { get; set; }
    }
}
