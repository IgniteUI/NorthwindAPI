namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class LoginInputModel : IUser
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
