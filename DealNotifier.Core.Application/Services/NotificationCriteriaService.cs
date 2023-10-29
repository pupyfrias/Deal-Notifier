using AutoMapper;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Catalog.Application.Services
{
    public class NotificationCriteriaService : GenericService<NotificationCriteria>, INotificationCriteriaService
    {
        public NotificationCriteriaService(INotificationCriteriaRepository repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
        }
    }
}