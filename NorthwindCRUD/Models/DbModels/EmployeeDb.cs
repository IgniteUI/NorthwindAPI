using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class EmployeeDb : IBaseDb, IEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Title { get; set; }

        public string TitleOfCourtesy { get; set; }

        public string BirthDate { get; set; }

        public string HireDate { get; set; }

        public string AddressId { get; set; }

        public AddressDb Address { get; set; }

        public string Notes { get; set; }

        public string AvatarUrl { get; set; }

        public int ReportsTo { get; set; }

        public ICollection<EmployeeTerritoryDb> EmployeesTerritories { get; set; } = new List<EmployeeTerritoryDb>();

        public ICollection<OrderDb> Orders { get; set; } = new List<OrderDb>();
    }
}
