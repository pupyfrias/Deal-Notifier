using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Services.Items
{
    public interface IItemValidationService
    {
        bool TitleContainsKeyword(string title, string keywords);
        bool ContainsWordUnlocked(string title);
        bool CanBeSaved(ItemCreateRequest itemCreate);
        bool CanItemBeUpdated(Item oldItem, ItemCreateRequest item);

    }
}