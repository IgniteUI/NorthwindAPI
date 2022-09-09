namespace NorthwindCRUD.Models.InputModels
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeInputModel : IEmployee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string TitleOfCourtesy { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string HireDate { get; set; }

        [Required]
        public string AddressId { get; set; }

        [Required]
        public AddressInputModel Address { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public string AvatarUrl { get; set; }
    }
}
