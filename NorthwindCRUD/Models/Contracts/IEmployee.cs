using NorthwindCRUD.Models.DbModels;

namespace NorthwindCRUD.Models.Contracts
{
    public interface IEmployee
    {
        int EmployeeId { get; }

        string LastName { get; set; }

        string FirstName { get; set; }

        string Title { get; set; }

        string TitleOfCourtesy { get; set; }

        string BirthDate { get; set; }

        string HireDate { get; set; }

        string Notes { get; set; }

        string AvatarUrl { get; set; }
    }
}
