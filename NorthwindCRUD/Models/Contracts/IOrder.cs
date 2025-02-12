using NorthwindCRUD.Models.Dtos;
using static NorthwindCRUD.Helpers.Enums;

namespace NorthwindCRUD.Models.Contracts
{
    public interface IOrder
    {
        int OrderId { get; set; }

        string CustomerId { get; set; }

        int EmployeeId { get; set; }

        int? ShipperId { get; set; }

        string OrderDate { get; set; }

        string RequiredDate { get; set; }

        Shipping? ShipVia { get; set; }

        double Freight { get; set; }

        string ShipName { get; set; }

        public bool Completed { get; set; }

        AddressDto ShipAddress { get; set; }
    }
}
