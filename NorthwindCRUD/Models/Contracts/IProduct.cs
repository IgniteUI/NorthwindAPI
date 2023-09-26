namespace NorthwindCRUD.Models.Contracts
{
    public interface IProduct
    {
        int ProductId { get; set; }

        int SupplierId { get; set; }

        int CategoryId { get; set; }

        string QuantityPerUnit { get; set; }

        double? UnitPrice { get; set; }

        int? UnitsInStock { get; set; }

        int? UnitsOnOrder { get; set; }

        int? ReorderLevel { get; set; }

        bool Discontinued { get; set; }
    }
}
