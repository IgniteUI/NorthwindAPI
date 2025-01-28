using System.ComponentModel.DataAnnotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class SalesDto : IBaseDto
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "QuantitySold must be at least 1.")]
        public int QuantitySold { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "SaleAmount must be greater than 0.")]
        public double SaleAmount { get; set; }
    }
}
