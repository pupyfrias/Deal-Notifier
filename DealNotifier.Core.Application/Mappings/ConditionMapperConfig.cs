using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.Condition;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class ConditionMapperConfig : Profile
    {
        public ConditionMapperConfig()
        {
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<ConditionCreateRequest, Condition>().IgnoreAllSourceNullProperties();
            CreateMap<ConditionUpdateRequest, Condition>().IgnoreAllSourceNullProperties();
            CreateMap<Condition, ConditionResponse>().IgnoreAllSourceNullProperties();
        }
    }
}