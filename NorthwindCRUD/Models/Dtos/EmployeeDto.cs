using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class EmployeeDto : IEmployee
    {
        [SwaggerSchema("Number automatically assigned to new employee.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [SwaggerSchema("Employee's last name.")]
        public string LastName { get; set; }

        [SwaggerSchema("Employee's first name.")]
        public string FirstName { get; set; }

        [SwaggerSchema("Employee's title")]
        public string Title { get; set; }

        [SwaggerSchema("Title used in salutations")]
        public string TitleOfCourtesy { get; set; }

        [DataType(DataType.Date, ErrorMessage = "BirthDate must be a valid date.")]
        [SwaggerSchema("Employee's birth date")]
        public string BirthDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "HireDate must be a valid date.")]
        [SwaggerSchema("Employee's hire date")]
        public string HireDate { get; set; }

        public string AddressId { get; set; }

        public AddressDto Address { get; set; }

        [SwaggerSchema("General information about employee's background.")]
        public string Notes { get; set; }

        [SwaggerSchema("Employee's avatar url.")]
        public string AvatarUrl { get; set; }

        [SwaggerSchema("Employee's supervisor.")]
        public int ReportsTo { get; set; }
    }
}
