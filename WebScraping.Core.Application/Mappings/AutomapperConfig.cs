﻿using AutoMapper;
using WebScraping.Core.Application.Dtos;
using WebScraping.Core.Application.Dtos.Condition;
using WebScraping.Core.Application.Dtos.Item;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Mappings
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Item, ItemResponseDto>().ReverseMap();
            CreateMap<Item, ItemClean>().ReverseMap();
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<BlackList, BlackListDto>().ReverseMap();
            CreateMap<Banned, BannedDto>().ReverseMap();
            CreateMap<BlackList, Item>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            //CreateMap<ApplicationUser, ApiUserDto>().ReverseMap();
        }
    }
}