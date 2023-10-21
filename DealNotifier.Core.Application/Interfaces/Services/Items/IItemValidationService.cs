using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services.Items
{
    public interface IItemValidationService
    {
        bool TitleContainsKeyword(string title, string keywords);
        bool ContainsWordUnlocked(string title);
        bool CanBeSaved(ItemCreateRequest itemCreate);
        bool CanItemBeUpdated(Item oldItem, ItemCreateRequest item);

    }
}