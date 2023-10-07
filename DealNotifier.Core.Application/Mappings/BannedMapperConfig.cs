using AutoMapper;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class BannedMapperConfig : Profile
    {
        public BannedMapperConfig()
        {
            CreateMap<BanKeyword, BannedDto>().ReverseMap();
        }
    }
}