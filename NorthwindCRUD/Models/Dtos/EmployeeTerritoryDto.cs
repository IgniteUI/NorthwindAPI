namespace NorthwindCRUD.Models.Dtos
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeTerritoryDto : IEmployeeTerritory
    {
        public int EmployeeId { get; set; }

        public string TerritoryId { get; set; }
    }
}
