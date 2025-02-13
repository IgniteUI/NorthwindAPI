using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class AssetDto : IAsset
    {
        public string Ticker { get; set; }

        public string HoldingName { get; set; }

        public double Positions { get; set; }

        public int HoldingPeriod { get; set; }

        public double CurrentPrice { get; set; }

        public double BoughtPrice { get; set; }
    }
}
