using AutoMapper;
using DealNotifier.Core.Application.ViewModels.V1.Unlockable;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class UnlockableMapperConfig : Profile
    {
        public UnlockableMapperConfig()
        {
            CreateMap<UnlockableCreateDto, UnlockabledPhone>().ReverseMap();
            CreateMap<UnlockableReadDto, UnlockabledPhone>().ReverseMap();
        }
    }
}