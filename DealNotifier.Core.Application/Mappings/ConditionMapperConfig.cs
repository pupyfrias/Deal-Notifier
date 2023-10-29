using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.Condition;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class ConditionMapperConfig : Profile
    {
        public ConditionMapperConfig()
        {
            CreateMap<ConditionCreateRequest, Condition>().IgnoreAllSourceNullProperties();
            CreateMap<ConditionUpdateRequest, Condition>().IgnoreAllSourceNullProperties();
            CreateMap<Condition, ConditionResponse>().IgnoreAllSourceNullProperties();
            CreateMap<Condition, ConditionDto>().ReverseMap();
        }
    }
}