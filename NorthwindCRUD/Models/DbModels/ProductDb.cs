using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class ProductDb : IBaseDb, IProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        public int? SupplierId { get; set; }

        public SupplierDb? Supplier { get; set; }

        public int? CategoryId { get; set; }

        public CategoryDb? Category { get; set; }

        public string QuantityPerUnit { get; set; }

        public double? UnitPrice { get; set; }

        public int? UnitsInStock { get; set; }

        public int? UnitsOnOrder { get; set; }

        public int? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public ICollection<OrderDetailDb> Details { get; set; } = new List<OrderDetailDb>();
    }
}
