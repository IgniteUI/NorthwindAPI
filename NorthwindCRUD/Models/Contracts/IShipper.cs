namespace NorthwindCRUD.Models.Contracts
{
    public interface IShipper
    {
         int ShipperId { get; }

         string CompanyName { get; set; }

         string Phone { get; set; }
    }
}
