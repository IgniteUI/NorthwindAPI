using System.ComponentModel.DataAnnotations;
using NorthwindCRUD.Models.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace NorthwindCRUD.Models.Dtos
{
    public class EmployeeDto : IEmployee
    {
        [SwaggerSchema("Number automatically assigned to new employee.")]
        [Required]
        public int EmployeeId { get; set; }

        [SwaggerSchema("Employee's last name.")]
        public string LastName { get; set; }

        [SwaggerSchema("Employee's first name.")]
        public string FirstName { get; set; }

        [SwaggerSchema("Employee's title")]
        public string Title { get; set; }

        [SwaggerSchema("Title used in salutations")]
        public string TitleOfCourtesy { get; set; }

        [SwaggerSchema("Employee's birth date", Format = "date")]
        public string BirthDate { get; set; }

        [SwaggerSchema("Employee's hire date", Format = "date")]
        public string HireDate { get; set; }

        public string AddressId { get; set; }

        public AddressDto Address { get; set; }

        [SwaggerSchema("General information about employee's background.")]
        public string Notes { get; set; }

        [SwaggerSchema("Employee's avatar url.", Format = "uri")]
        public string AvatarUrl { get; set; }

        [SwaggerSchema("Employee's supervisor.")]
        public int ReportsTo { get; set; }
    }
}
