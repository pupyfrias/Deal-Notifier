using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.BanKeyword;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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