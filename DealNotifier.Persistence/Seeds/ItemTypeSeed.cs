
using Catalog.Domain.Entities;
using Enums = Catalog.Application.Enums;


namespace Catalog.Persistence.Seeds
{
    public static class ItemTypeSeed
    {
        public static List<ItemType> Data { get; } =
            Enum.GetValues<Enums.ItemType>()
            .Select(e => new ItemType { Id = (int)e, Name = e.ToString() })
            .ToList();
    }
}