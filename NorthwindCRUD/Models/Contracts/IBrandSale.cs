namespace NorthwindCRUD.Models.Contracts
{
    public interface IBrandSale
    {
        string Store { get; set; }

        string Brand { get; set; }

        string Country { get; set; }

        double Sale { get; set; }

        double Cost { get; set; }

        string? SaleDate { get; set; }
    }
}
