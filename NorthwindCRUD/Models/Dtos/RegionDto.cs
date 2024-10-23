using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class RegionDto : IBaseDto, IRegion
    {
        [Key]
        public int RegionId { get; set; }

        public string RegionDescription { get; set; }
    }
}
