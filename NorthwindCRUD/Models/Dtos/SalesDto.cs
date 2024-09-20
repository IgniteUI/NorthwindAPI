namespace NorthwindCRUD.Models.Dtos
{
    public class SalesDto : IBaseDto
    {
        public int ProductId { get; set; }

        public int QuantitySold { get; set; }

        public double SaleAmount { get; set; }
    }
}
