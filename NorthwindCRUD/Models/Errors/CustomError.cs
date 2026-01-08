namespace NorthwindCRUD.Models.Errors
{
    public class CustomError
    {
        public CustomError()
        {
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}