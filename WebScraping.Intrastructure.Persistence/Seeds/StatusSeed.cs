using WebScraping.Core.Domain.Entities;
using Emuns = WebScraping.Core.Application.Emuns;
namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public class StatusSeed
    {
        public static List<Status> data = new List<Status>
        {
            new Status
            {
                Id= (int) Emuns.Status.InStock,
                Name= Emuns.Status.InStock.ToString()
                
            },
            new Status
            {
                Id= (int) Emuns.Status.OutStock,
                Name= Emuns.Status.OutStock.ToString()
            }
        };
    }
}
