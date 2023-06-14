using AutoMapper;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Application.DTOs.Condition;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Item, ItemResponseDto>().ReverseMap();
            /*CreateMap<Item, ItemCreateDto>().ReverseMap();*/
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<ConditionsToNotify, ConditionsToNotifyDto>().ReverseMap();
            CreateMap<BlackList, BlackListDto>().ReverseMap();
            CreateMap<Banned, BannedDto>().ReverseMap();
            CreateMap<Brand, BrandReadDto>().ReverseMap();
            CreateMap<BlackList, Item>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<BlackList, Item>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            //CreateMap<ApplicationUser, ApiUserDto>().ReverseMap();
        }
    }
}