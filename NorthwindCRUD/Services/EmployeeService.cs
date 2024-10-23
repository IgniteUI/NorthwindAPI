namespace NorthwindCRUD.Services
{
    using AutoMapper;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;

    public class EmployeeService : BaseDbService<EmployeeDto, EmployeeDb, int>
    {
        public EmployeeService(DataContext dataContext, IPagingService pagingService, IMapper mapper)
            : base(dataContext, mapper, pagingService)
        {
        }

        public EmployeeDto[] GetEmployeesByReportsTo(int id)
        {
            return mapper.Map<EmployeeDto[]>(this.dataContext.Employees
                .Where(c => c.ReportsTo == id)
                .ToArray());
        }
    }
}
