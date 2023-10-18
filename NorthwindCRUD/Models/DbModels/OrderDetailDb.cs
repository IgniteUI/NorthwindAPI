namespace NorthwindCRUD.Models.DbModels
{
    public class OrderDetailDb
    {
        public int OrderId { get; set; }

        public OrderDb Order { get; set; }

        public int ProductId { get; set; }

        public ProductDb Product { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public float Discount { get; set; }
    }
}
