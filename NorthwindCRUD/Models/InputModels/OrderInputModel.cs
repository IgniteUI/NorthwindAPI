namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class OrderInputModel : IOrder
    {
        public int OrderId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string OrderDate { get; set; }

        [Required]
        public string RequiredDate { get; set; }

        [Required]
        public int ShipVia { get; set; }

        [Required]
        public double Freight { get; set; }

        [Required]
        public string ShipName { get; set; }

        public string ShipAddressId { get; set; }

        [Required]
        public AddressInputModel ShipAddress { get; set; }
    }
}
