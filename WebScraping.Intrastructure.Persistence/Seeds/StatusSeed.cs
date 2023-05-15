using WebScraping.Core.Domain.Entities;
using Enums = WebScraping.Core.Application.Enums;

namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public static class StatusSeed
    {
        public static List<Status> data { get; set; } = new List<Status>
        {
            new Status
            {
                Id= (int) Enums.Status.InStock,
                Name= Enums.Status.InStock.ToString()
            },
            new Status
            {
                Id= (int) Enums.Status.OutStock,
                Name= Enums.Status.OutStock.ToString()
            }
        };
    }
}