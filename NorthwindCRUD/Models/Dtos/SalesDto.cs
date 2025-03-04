using System.ComponentModel.DataAnnotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class SalesDto
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        public int QuantitySold { get; set; }

        public double SaleAmount { get; set; }
    }
}
