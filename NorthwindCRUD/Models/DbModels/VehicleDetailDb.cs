using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class VehicleDetailDb : IVehicleDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DetailId { get; set; }

        public string Generation { get; set; }

        public int YearOfManufacture { get; set; }

        public string FuelType { get; set; }

        public int Doors { get; set; }

        public int Seats { get; set; }

        public string Transmission { get; set; }

        public string Engine { get; set; }

        public string Power { get; set; }

        public string Mileage { get; set; }

        public string Cubature { get; set; }

        public string Color { get; set; }

        public string Msrp { get; set; }

        public string TollPassId { get; set; }

        public VehicleDb Vehicle { get; set; }

        public string VehicleId { get; set; }
    }
}
