using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;


namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class ItemTypeSeed
    {
        public static List<ItemType> Data { get;} = Enum.GetValues<Enums.ItemType>()
                                                    .Select(e => new ItemType { Id = (int)e, Name = e.ToString() })
                                                    .ToList();
    }
}