using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Services.Items
{
    public interface IItemNotificationService
    {
        Task NotifyUsersOfItemsByEmail();
        void EvaluateIfNotifiable(Item item);
    }
}