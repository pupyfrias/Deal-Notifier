using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.NotificationCriteria;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
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