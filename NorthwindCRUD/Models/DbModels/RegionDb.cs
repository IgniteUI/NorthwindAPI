using NorthwindCRUD.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindCRUD.Models.DbModels
{
    public class RegionDb : IRegion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RegionId { get; set; }

        public string RegionDescription { get; set; }

        public ICollection<TerritoryDb> Territories { get; set; } = new List<TerritoryDb>();
    }
}
