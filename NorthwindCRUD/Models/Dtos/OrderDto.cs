using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;
using static NorthwindCRUD.Helpers.Enums;

namespace NorthwindCRUD.Models.Dtos
{
    public class OrderDto : IOrder
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "CustomerId is required.")]
        public string? CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public int? ShipperId { get; set; }

        [DataType(DataType.Date, ErrorMessage = "OrderDate must be a valid date.")]
        public string OrderDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "RequiredDate must be a valid date.")]
        public string RequiredDate { get; set; }

        public Shipping? ShipVia { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public bool Completed { get; set; }

        public AddressDto ShipAddress { get; set; }
    }
}
