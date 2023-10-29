using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.BanLink;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class BanLinkMapperConfig : Profile
    {
        public BanLinkMapperConfig()
        {
            CreateMap<BanLinkCreateRequest, BanLink>().IgnoreAllSourceNullProperties();
            CreateMap<BanLinkUpdateRequest, BanLink>().IgnoreAllSourceNullProperties();
            CreateMap<BanLink, BanLinkResponse>().IgnoreAllSourceNullProperties();
            CreateMap<BanLink, BanLinkDto>().IgnoreAllSourceNullProperties();
        }
    }
}