using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Persistence.Seeds
{
    public static class UnlockToolSeed
    {
        public static List<PhoneUnlockTool> Data { get; } = new List<PhoneUnlockTool>
        {
            new PhoneUnlockTool
            {
                Id= (int) Enums.UnlockTool.TUnlock,
                Name= "T-Unlock"
            },
            new PhoneUnlockTool
            {
                Id= (int) Enums.UnlockTool.SamKey,
                Name= "SamKey"
            },
             new PhoneUnlockTool
             {
                Id= (int) Enums.UnlockTool.GlobalUnlocker,
                Name= "Global Unlocker"
             }
        };
    }
}