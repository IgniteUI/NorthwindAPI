namespace NorthwindCRUD
{
    public class AuthenticationError
    {
        public AuthenticationError()
        {
            int statusCode = 401;
            string message = "No! No! You are not authenticated!";
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}