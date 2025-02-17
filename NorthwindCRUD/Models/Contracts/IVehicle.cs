namespace NorthwindCRUD.Models.Contracts
{
    public interface IVehicle
    {
        string VehicleId { get; set; }

        string LicensePlate { get; set; }

        string Make { get; set; }

        string Model { get; set; }

        string Type { get; set; }

        string Vin { get; set; }

        string Status { get; set; }

        string LocationCity { get; set; }

        string LocationGps { get; set; }
    }
}
