using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Contracts
{
    public interface ICustomer
    {
        string CustomerId { get; set; }

        string? CompanyName { get; set; }

        string ContactName { get; set; }

        string ContactTitle { get; set; }
    }
}
