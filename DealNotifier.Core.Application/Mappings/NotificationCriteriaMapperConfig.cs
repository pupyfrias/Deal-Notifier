using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.NotificationCriteria;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class NotificationCriteriaMapperConfig : Profile
    {
        public NotificationCriteriaMapperConfig()
        {
            CreateMap<NotificationCriteriaCreateRequest, NotificationCriteria>().IgnoreAllSourceNullProperties();
            CreateMap<NotificationCriteriaUpdateRequest, NotificationCriteria>().IgnoreAllSourceNullProperties();
            CreateMap<NotificationCriteria, NotificationCriteriaResponse>().IgnoreAllSourceNullProperties();
            CreateMap<NotificationCriteria, NotificationCriteriaDto>().ReverseMap();
        }
    }
}