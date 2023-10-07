using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class ShopSeed
    {
        public static List<Shop> Data { get;} = new List<Shop>
        {
            new Shop
            {
                Id= (int) Enums.Shop.Amazon,
                Name= Enums.Shop.Amazon.ToString()
            },
            new Shop
            {
                Id= (int) Enums.Shop.eBay,
                Name= Enums.Shop.eBay.ToString()
            },
             new Shop
             {
                Id= (int) Enums.Shop.TheStore,
                Name= Enums.Shop.TheStore.ToString()
             }
        };
    }
}