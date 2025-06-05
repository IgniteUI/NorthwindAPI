using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Contracts
{
    public interface ICustomer
    {
        string CustomerId { get; }

        string CompanyName { get; set; }

        string ContactName { get; set; }

        string ContactTitle { get; set; }
    }
}
