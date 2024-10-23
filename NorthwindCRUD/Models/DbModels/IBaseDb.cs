namespace NorthwindCRUD.Models.DbModels
{
    public interface IBaseDb
    {
        public string[] GetIncludes()
        {
            return Array.Empty<string>();
        }
    }
}
