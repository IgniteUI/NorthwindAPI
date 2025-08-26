using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class AddressDb : IAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AddressId { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public ICollection<CustomerDb> Customers { get; set; }

        public ICollection<EmployeeDb> Employees { get; set; }

        public ICollection<OrderDb> Orders { get; set; }
    }
}
