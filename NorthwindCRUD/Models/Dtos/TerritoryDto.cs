using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class TerritoryDto : ITerritory
    {
        public string TerritoryId { get; private set; }

        public string TerritoryDescription { get; set; }

        public int? RegionId { get; set; }
    }
}
