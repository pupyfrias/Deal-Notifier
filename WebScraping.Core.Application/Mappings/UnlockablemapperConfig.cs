using AutoMapper;
using WebScraping.Core.Application.Dtos.Unlockable;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Mappings
{
    public class UnlockablemapperConfig : Profile
    {
        public UnlockablemapperConfig()
        {
            CreateMap<UnlockableCreateDto, Unlockable>().ReverseMap();
            
        }
    }
}