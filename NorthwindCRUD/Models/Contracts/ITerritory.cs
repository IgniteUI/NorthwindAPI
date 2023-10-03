namespace NorthwindCRUD.Models.Contracts
{
    public interface ITerritory
    {
        string TerritoryId { get; set; }

        string TerritoryDescription { get; set; }

        public int? RegionId { get; set; }
    }
}
