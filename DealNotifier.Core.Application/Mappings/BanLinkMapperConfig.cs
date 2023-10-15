using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.BanLink;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
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