using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class OnlineStoreSeed
    {
        public static List<OnlineStore> Data { get;} = Enum.GetValues<Enums.OnlineStore>()
                                                       .Select(e => new OnlineStore { Id = (int)e, Name = e.ToString() })
                                                       .ToList();
    }
}