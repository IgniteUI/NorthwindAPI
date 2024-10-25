namespace NorthwindCRUD.Models.DbModels
{
    public class EmployeeTerritoryDb : IBaseDb
    {
        public int EmployeeId { get; set; }

        public EmployeeDb Employee { get; set; }

        public string TerritoryId { get; set; }

        public TerritoryDb Territory { get; set; }
    }
}
