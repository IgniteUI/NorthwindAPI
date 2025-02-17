using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class BrandSaleDto : IBrandSale
    {
        public string Store { get; set; }

        public string Brand { get; set; }

        public string Country { get; set; }

        public double Sale { get; set; }

        public double Cost { get; set; }

        public string? SaleDate { get; set; }
    }
}
