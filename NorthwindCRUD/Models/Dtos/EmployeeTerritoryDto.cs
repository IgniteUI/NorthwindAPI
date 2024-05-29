using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class EmployeeTerritoryDto : IEmployeeTerritory
    {
        public int EmployeeId { get; set; }

        public string TerritoryId { get; set; }
    }
}
