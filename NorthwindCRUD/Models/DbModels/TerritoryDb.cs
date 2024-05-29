using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class TerritoryDb : ITerritory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string TerritoryId { get; set; }

        public int? RegionId { get; set; }

        public RegionDb? Region { get; set; }

        public string TerritoryDescription { get; set; }

        public ICollection<EmployeeTerritoryDb> EmployeesTerritories { get; set; } = new List<EmployeeTerritoryDb>();
    }
}
