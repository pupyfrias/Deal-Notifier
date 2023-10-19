using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Helpers;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Extensions
{
    public static class ItemExtension
    {
        /// <summary>
        /// Validate if the Item can be save.
        /// </summary>
        /// <returns>true if the Item is not in BlackList; otherwise, false.</returns>


        

        /// <summary>
        /// Set Item Condition
        /// </summary>
        public static void SetCondition(this ItemCreateRequest item)
        {
            /* bool isNotNew =  conditionList.Any(condition =>  item.Name.IndexOf(condition, StringComparison.OrdinalIgnoreCase) >= 0);
             item.ConditionId = isNotNew ? (int)Condition.Used : (int)Condition.New;*/
        }
    }
}