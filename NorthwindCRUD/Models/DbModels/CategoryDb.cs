namespace NorthwindCRUD.Models.DbModels
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CategoryDb : ICategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }
    }
}
