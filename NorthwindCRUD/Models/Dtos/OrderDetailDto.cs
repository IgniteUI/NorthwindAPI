using System.ComponentModel.DataAnnotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class OrderDetailDto
    {
        [Required(ErrorMessage = "OrderId is required.")]
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public double UnitPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public float Discount { get; set; }
    }
}
