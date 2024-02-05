using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface INotificationCriteriaRepository : IAsyncRepository<NotificationCriteria>
    {
    }
}