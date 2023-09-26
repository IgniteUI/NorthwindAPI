using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class TerritoryDto : ITerritory
    {
        public string TerritoryId { get; set; }

        public string TerritoryDescription { get; set; }
    }
}
