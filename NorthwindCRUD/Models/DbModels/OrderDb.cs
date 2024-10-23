using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindCRUD.Models.DbModels
{
    public class OrderDb : IBaseDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string? CustomerId { get; set; }

        public CustomerDb? Customer { get; set; }

        public int? EmployeeId { get; set; }

        public EmployeeDb? Employee { get; set; }

        public int? ShipperId { get; set; }

        public ShipperDb? Shipper { get; set; }

        public string OrderDate { get; set; }

        public string RequiredDate { get; set; }

        public int ShipVia { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public float Discount { get; set; }

        public AddressDb? ShipAddress { get; set; }
    }
}
