using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
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