using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class ProductDto : IProduct
    {
        public int ProductId { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public string? QuantityPerUnit { get; set; }

        public double? UnitPrice { get; set; }

        public int? UnitsInStock { get; set; }

        public int? UnitsOnOrder { get; set; }

        public int? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
