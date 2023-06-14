using AutoMapper;
using DealNotifier.Core.Application.DTOs.Unlockable;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class UnlockablemapperConfig : Profile
    {
        public UnlockablemapperConfig()
        {
            CreateMap<UnlockableCreateDto, UnlockablePhone>().ReverseMap();
            CreateMap<UnlockableReadDto, UnlockablePhone>().ReverseMap();

        }
    }
}