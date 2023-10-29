using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.Brand;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class BrandMapperConfig : Profile
    {
        public BrandMapperConfig()
        {
            CreateMap<BrandCreateRequest, Brand>().IgnoreAllSourceNullProperties();
            CreateMap<BrandUpdateRequest, Brand>().IgnoreAllSourceNullProperties();
            CreateMap<Brand, BrandResponse>().IgnoreAllSourceNullProperties();
            CreateMap<Brand, BrandDto>().ReverseMap();
        }
    }
}