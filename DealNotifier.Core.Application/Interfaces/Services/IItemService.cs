using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IItemService : IGenericService<Item, Guid>
    {
    }
}