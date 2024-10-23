using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class EmployeeTerritoryDto : IBaseDto, IEmployeeTerritory
    {
        [Key]
        public int EmployeeId { get; set; }

        public string TerritoryId { get; set; }
    }
}
