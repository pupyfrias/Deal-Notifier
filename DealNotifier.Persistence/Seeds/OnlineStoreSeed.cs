
using Catalog.Domain.Entities;
using Enums = Catalog.Application.Enums;

namespace Catalog.Persistence.Seeds
{
    public static class OnlineStoreSeed
    {
        public static List<OnlineStore> Data { get; } =
            Enum.GetValues<Enums.OnlineStore>()
            .Select(e => new OnlineStore { Id = (int)e, Name = e.ToString() })
            .ToList();
    }
}