﻿namespace NorthwindCRUD.Models.Dtos
{
    using NorthwindCRUD.Models.Contracts;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeDto : IEmployee
    {
        public int EmployeeId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Title { get; set; }

        public string TitleOfCourtesy { get; set; }

        public string BirthDate { get; set; }

        public string HireDate { get; set; }

        public string AddressId { get; set; }

        public AddressDto Address { get; set; }

        public string Notes { get; set; }

        public string AvatarUrl { get; set; }

        public int ReportsTo { get; set; }
    }
}
