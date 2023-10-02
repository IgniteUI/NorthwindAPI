namespace NorthwindCRUD.Exceptions
{
    public class InvalidEntityIdException : Exception
    {
        public InvalidEntityIdException(string name, string id) : base($"{name} with id {id} does not exist!")
        {
        }
    }
}
