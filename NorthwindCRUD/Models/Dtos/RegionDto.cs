using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class RegionDto : IRegion
    {
        public int RegionId { get; private set; }

        public string RegionDescription { get; set; }
    }
}
