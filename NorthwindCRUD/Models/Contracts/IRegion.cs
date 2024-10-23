namespace NorthwindCRUD.Models.Contracts
{
    public interface IRegion
    {
        int RegionId { get; }

        string RegionDescription { get; set; }
    }
}
