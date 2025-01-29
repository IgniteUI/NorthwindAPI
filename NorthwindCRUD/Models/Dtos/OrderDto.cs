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

        public int EmployeeId { get; set; }

        public int ShipperId { get; set; }

        public string OrderDate { get; set; }

        public string RequiredDate { get; set; }

        public Shipping? ShipVia { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public bool Completed { get; set; }

        public AddressDto ShipAddress { get; set; }
    }
}
