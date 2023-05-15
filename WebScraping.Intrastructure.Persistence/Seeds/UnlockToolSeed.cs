using WebScraping.Core.Domain.Entities;
using Enums = WebScraping.Core.Application.Enums;

namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public static class UnlockToolSeed
    {
        public static List<UnlockTool> data { get; set; } = new List<UnlockTool>
        {
            new UnlockTool
            {
                Id= (int) Enums.UnlockTool.TUnlock,
                Name= "T-Unlock"
            },
            new UnlockTool
            {
                Id= (int) Enums.UnlockTool.SamKey,
                Name= "SamKey"
            },
             new UnlockTool
             {
                Id= (int) Enums.UnlockTool.GlobalUnlocker,
                Name= "Global Unlocker"
             }
        };
    }
}