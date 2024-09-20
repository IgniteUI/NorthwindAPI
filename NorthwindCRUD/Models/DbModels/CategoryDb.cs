using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class CategoryDb : IBaseDb, ICategory, ICategoryDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public ICollection<ProductDb> Products { get; set; } = new List<ProductDb>();
    }
}
