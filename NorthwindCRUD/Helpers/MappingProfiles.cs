namespace NorthwindCRUD.Helpers
{
    using AutoMapper;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CategoryInputModel, CategoryDb>().ReverseMap();
            CreateMap<CustomerInputModel, CustomerDb>().ReverseMap();
            CreateMap<EmployeeInputModel, EmployeeDb>().ReverseMap();
            CreateMap<OrderInputModel, OrderDb>().ReverseMap();
            CreateMap<AddressInputModel, AddressDb>().ReverseMap();
        }
    }
}
