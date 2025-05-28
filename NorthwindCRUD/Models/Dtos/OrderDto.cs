using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;
using static NorthwindCRUD.Helpers.Enums;

namespace NorthwindCRUD.Models.Dtos
{
    public class OrderDto : IOrder
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "CustomerId is required.")]
        public string CustomerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId must be a valid employee ID.")]
        public int EmployeeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ShipperId must be a valid shipper ID.")]
        public int? ShipperId { get; set; }

        [DataType(DataType.Date, ErrorMessage = "OrderDate must be a valid date.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string OrderDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "RequiredDate must be a valid date.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string RequiredDate { get; set; }

        public Shipping? ShipVia { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Freight must be a non-negative value.")]
        public double Freight { get; set; }

        [StringLength(100, ErrorMessage = "ShipName cannot exceed 100 characters.")]
        public string ShipName { get; set; }

        public bool Completed { get; set; }

        public AddressDto ShipAddress { get; set; }
    }
}
