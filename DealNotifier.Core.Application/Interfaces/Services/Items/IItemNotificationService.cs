using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services.Items
{
    public interface IItemNotificationService
    {
        Task NotifyUsersOfItemsByEmail();
        void EvaluateIfNotifiable(Item item);
    }
}