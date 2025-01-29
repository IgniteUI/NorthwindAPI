using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class LoginDto : IUser
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
