using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Models.Contracts
{
    public interface IOrder
    {
        int OrderId { get; }

        string CustomerId { get; set; }

        int EmployeeId { get; set; }

        int ShipperId { get; set; }

        string OrderDate { get; set; }

        string RequiredDate { get; set; }

        int ShipVia { get; set; }

        double Freight { get; set; }

        string ShipName { get; set; }

        AddressDto ShipAddress { get; set; }
    }
}
