using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.BanKeyword;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class BanKeywordMapperConfig : Profile
    {
        public BanKeywordMapperConfig()
        {
            CreateMap<BanKeywordCreateRequest, BanKeyword>().IgnoreAllSourceNullProperties();
            CreateMap<BanKeywordUpdateRequest, BanKeyword>().IgnoreAllSourceNullProperties();
            CreateMap<BanKeyword, BanKeywordResponse>().IgnoreAllSourceNullProperties();
        }
    }
}