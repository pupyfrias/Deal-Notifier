using AutoMapper;
using Catalog.Application.ViewModels.V1.BanKeyword;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class BannedMapperConfig : Profile
    {
        public BannedMapperConfig()
        {
            CreateMap<BanKeyword, BanKeywordDto>().ReverseMap();
        }
    }
}