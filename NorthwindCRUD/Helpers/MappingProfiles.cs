using AutoMapper;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Entity to Entity (Ignoring ID fields)
        CreateMap<CategoryDb, CategoryDb>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CustomerDb, CustomerDb>()
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<SupplierDb, SupplierDb>()
            .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<EmployeeDb, EmployeeDb>()
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<TerritoryDb, TerritoryDb>()
            .ForMember(dest => dest.TerritoryId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<RegionDb, RegionDb>()
            .ForMember(dest => dest.RegionId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<OrderDb, OrderDb>()
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ProductDb, ProductDb>()
           .ForMember(dest => dest.ProductId, opt => opt.Ignore())
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ShipperDb, ShipperDb>()
           .ForMember(dest => dest.ShipperId, opt => opt.Ignore())
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // DTO to DB and Reverse mappings
        CreateMap<CategoryDto, CategoryDb>().ReverseMap();
        CreateMap<CategoryDb, CategoryDetailsDto>();
        CreateMap<ProductDto, ProductDb>().ReverseMap();
        CreateMap<RegionDto, RegionDb>().ReverseMap();
        CreateMap<ShipperDto, ShipperDb>().ReverseMap();
        CreateMap<SupplierDto, SupplierDb>().ReverseMap();
        CreateMap<TerritoryDto, TerritoryDb>().ReverseMap();
        CreateMap<CustomerDto, CustomerDb>().ReverseMap();
        CreateMap<OrderDto, OrderDb>().ReverseMap();
        CreateMap<OrderDetailDto, OrderDetailDb>().ReverseMap();
        CreateMap<AddressDto, AddressDb>().ReverseMap();
        CreateMap<LoginDto, UserDb>().ReverseMap();
        CreateMap<RegisterDto, UserDb>().ReverseMap();
        CreateMap<EmployeeDto, EmployeeDb>().ReverseMap();
        CreateMap<EmployeeTerritoryDto, EmployeeTerritoryDb>().ReverseMap();
    }
}
