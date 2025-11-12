using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Contracts
{
    public interface IAddress
    {
        string? Street { get; set; }

        string? City { get; set; }

        string? Region { get; set; }

        string? PostalCode { get; set; }

        string? Country { get; set; }

        string? Phone { get; set; }
    }
}
