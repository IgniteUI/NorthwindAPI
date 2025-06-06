namespace NorthwindCRUD.Models.Contracts
{
    public interface ICategory
    {
        int CategoryId { get; }

        string Description { get; set; }

        string Name { get; set; }
    }
}
