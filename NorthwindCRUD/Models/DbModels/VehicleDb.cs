using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class VehicleDb : IVehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string VehicleId { get; set; }

        public string LicensePlate { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Type { get; set; }

        public string Vin { get; set; }

        public string Status { get; set; }

        public string LocationCity { get; set; }

        public string LocationGps { get; set; }

        public VehicleDetailDb Details { get; set; }
    }
}
