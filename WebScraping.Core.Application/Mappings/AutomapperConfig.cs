using AutoMapper;
using WebScraping.Core.Application.DTOs;
using WebScraping.Core.Application.DTOs.Condition;
using WebScraping.Core.Application.DTOs.Item;
using WebScraping.Core.Application.DTOs.User;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Mappings
{
    public class AutomapperConfig: Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Item,ItemResponseDTO>().ReverseMap();
            CreateMap<Item,ItemClean>().ReverseMap();
            CreateMap<Condition,ConditionDTO>().ReverseMap();
            CreateMap<BlackList,BlackListDTO>().ReverseMap();
            //CreateMap<ApplicationUser, ApiUserDTO>().ReverseMap();
        }
    }
}
