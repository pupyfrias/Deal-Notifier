using AutoMapper;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Condition;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<NotificationCriteria, ConditionsToNotifyDto>().ReverseMap();
            CreateMap<BanLink, BanLink>().ReverseMap();
            CreateMap<Brand, BrandReadDto>().ReverseMap();
        }
    }
}