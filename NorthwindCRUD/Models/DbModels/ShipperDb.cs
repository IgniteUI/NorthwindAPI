using NorthwindCRUD.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindCRUD.Models.DbModels
{
    public class ShipperDb : IShipper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipperId { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }

        public ICollection<OrderDb> Orders { get; set; } =  new List<OrderDb>();
    }
}
