namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class OrderInputModel : IOrder
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public string OrderDate { get; set; }

        public string RequiredDate { get; set; }

        public int ShipVia { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddressId { get; set; }

        public AddressInputModel ShipAddress { get; set; }
    }
}
