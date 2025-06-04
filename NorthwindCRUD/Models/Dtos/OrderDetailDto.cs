using System.ComponentModel.DataAnnotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class OrderDetailDto
    {
        [Required(ErrorMessage = "OrderId is required.")]
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public float Discount { get; set; }
    }
}
