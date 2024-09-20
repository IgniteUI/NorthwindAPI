namespace NorthwindCRUD.Models.Dtos
{
    public class OrderDetailDto : IBaseDto
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public float Discount { get; set; }
    }
}
