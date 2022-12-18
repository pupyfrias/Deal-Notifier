using WebScraping.Core.Domain.Entities;
using Emuns = WebScraping.Core.Application.Emuns;
namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public class ShopSeed
    {
        public static List<Shop> data = new List<Shop>
        {
            new Shop
            {
                Id= (int) Emuns.Shop.Amazon,
                Name= Emuns.Shop.Amazon.ToString()
            },
            new Shop
            {
                Id= (int) Emuns.Shop.eBay,
                Name= Emuns.Shop.eBay.ToString()
            },
             new Shop
             {
                Id= (int) Emuns.Shop.TheStore,
                Name= Emuns.Shop.TheStore.ToString()
             }
        };
    }
}
