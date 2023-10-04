using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Contracts
{
    public interface IEmployeeTerritory
    {
        int EmployeeId { get; set; }

        string TerritoryId { get; set; }
    }
}