using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class OnlineStoreSeed
    {
        public static List<OnlineStore> Data { get;} = new List<OnlineStore>
        {
            new OnlineStore
            {
                Id= (int) Enums.OnlineStore.Amazon,
                Name= Enums.OnlineStore.Amazon.ToString()
            },
            new OnlineStore
            {
                Id= (int) Enums.OnlineStore.eBay,
                Name= Enums.OnlineStore.eBay.ToString()
            },
             new OnlineStore
             {
                Id= (int) Enums.OnlineStore.TheStore,
                Name= Enums.OnlineStore.TheStore.ToString()
             }
        };
    }
}