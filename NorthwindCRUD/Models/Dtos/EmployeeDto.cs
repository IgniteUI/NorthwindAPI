using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class EmployeeDto : IEmployee
    {
        [SwaggerSchema("Number automatically assigned to new employee.")]
        public int EmployeeId { get; private set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [SwaggerSchema("Employee's last name.")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [SwaggerSchema("Employee's first name.")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        [SwaggerSchema("Employee's title")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "Title of courtesy cannot exceed 50 characters.")]
        [SwaggerSchema("Title used in salutations")]
        public string TitleOfCourtesy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "BirthDate must be a valid date.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [SwaggerSchema("Employee's birth date")]
        public string BirthDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "HireDate must be a valid date.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [SwaggerSchema("Employee's hire date")]
        public string HireDate { get; set; }

        public string AddressId { get; set; }

        public AddressDto Address { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        [SwaggerSchema("General information about employee's background.")]
        public string Notes { get; set; }

        [RegularExpression(@"^https?:\/\/.+\..+", ErrorMessage = "Avatar URL is not valid.")]
        [SwaggerSchema("Employee's avatar url.")]
        public string AvatarUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ReportsTo must be a valid employee ID.")]
        [SwaggerSchema("Employee's supervisor.")]
        public int ReportsTo { get; set; }
    }
}
