namespace NorthwindCRUD.Models.Contracts
{
    public interface IAsset
    {
        string Ticker { get; set; }

        string HoldingName { get; set; }

        double Positions { get; set; }

        int HoldingPeriod { get; set; }

        double CurrentPrice { get; set; }

        double BoughtPrice { get; set; }
    }
}
