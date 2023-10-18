namespace NorthwindCRUD.Models.Contracts
{
    public interface IShipper
    {
         int ShipperId { get; set; }

         string CompanyName { get; set; }

         string Phone { get; set; }
    }
}
