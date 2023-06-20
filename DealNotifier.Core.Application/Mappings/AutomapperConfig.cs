using AutoMapper;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Application.DTOs.Condition;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<ConditionsToNotify, ConditionsToNotifyDto>().ReverseMap();
            CreateMap<BlackList, BlackListDto>().ReverseMap();
            CreateMap<Brand, BrandReadDto>().ReverseMap();
        }
    }
}