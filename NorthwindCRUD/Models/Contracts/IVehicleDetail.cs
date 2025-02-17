namespace NorthwindCRUD.Models.Contracts
{
    public interface IVehicleDetail
    {
        string Generation { get; set; }

        int YearOfManufacture { get; set; }

        string FuelType { get; set; }

        int Doors { get; set; }

        int Seats { get; set; }

        string Transmission { get; set; }

        string Engine { get; set; }

        string Power { get; set; }

        string Mileage { get; set; }

        string Cubature { get; set; }

        string Color { get; set; }

        string Msrp { get; set; }

        string TollPassId { get; set; }
    }
}
