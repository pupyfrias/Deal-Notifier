using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services.Items
{
    public interface IItemValidationService
    {
        bool DescriptionMatchesIncludeExcludeCriteria(string description, string includeKeywords, string excludeKeywords);
        bool ContainsWordUnlocked(string description);
        bool CanBeSaved(ItemDto itemCreate);
        bool CanItemBeUpdated(Item oldItem, ItemDto item);

    }
}