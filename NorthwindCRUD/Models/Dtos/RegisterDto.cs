namespace NorthwindCRUD.Models.Dtos
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class RegisterDto : IUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmedPassword { get; set; }
    }
}
