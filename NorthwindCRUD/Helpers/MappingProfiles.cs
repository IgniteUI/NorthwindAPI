namespace NorthwindCRUD.Helpers
{
    using AutoMapper;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
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
        }
    }
}
