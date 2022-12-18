using AutoMapper;
using WebScraping.Core.Application.DTOs.Condition;
using WebScraping.Core.Application.DTOs.Item;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Mappings
{
    public class AutomapperConfig: Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Item,ItemResponseDTO>().ReverseMap();
            CreateMap<Condition,ConditionDTO>().ReverseMap();
        }
    }
}
