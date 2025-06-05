using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class ProductDto : IProduct
    {
        public int ProductId { get; private set; }

        [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a valid supplier ID.")]
        public int? SupplierId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a valid category ID.")]
        public int? CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "QuantityPerUnit cannot exceed 100 characters.")]
        public string QuantityPerUnit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "UnitPrice must be a non-negative value.")]
        public double? UnitPrice { get; set; }

        [Range(0, 1000000000, ErrorMessage = "UnitsInStock must be a non-negative value.")]
        public int? UnitsInStock { get; set; }

        [Range(0, 1000000, ErrorMessage = "UnitsOnOrder must be a non-negative value.")]
        public int? UnitsOnOrder { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ReorderLevel must be a non-negative value.")]
        public int? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
