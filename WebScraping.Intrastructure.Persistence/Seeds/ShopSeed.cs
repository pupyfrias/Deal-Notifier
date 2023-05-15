using WebScraping.Core.Domain.Entities;
using Enums = WebScraping.Core.Application.Enums;

namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public static class ShopSeed
    {
        public static List<Shop> data { get; set; } = new List<Shop>
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