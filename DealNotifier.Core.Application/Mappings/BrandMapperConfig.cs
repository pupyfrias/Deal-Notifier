using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Brand;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
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