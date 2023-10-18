namespace NorthwindCRUD.Models.Dtos
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class LoginDto : IUser
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
