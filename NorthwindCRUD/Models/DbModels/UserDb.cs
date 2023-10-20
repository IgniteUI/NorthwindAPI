using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class UserDb : IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}