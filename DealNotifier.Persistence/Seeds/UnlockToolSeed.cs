using Catalog.Application.Enums;
using Catalog.Domain.Entities;

namespace Catalog.Persistence.Seeds
{
    public static class UnlockToolSeed
    {
        public static List<PhoneUnlockTool> Data { get; } = new List<PhoneUnlockTool>
        {
            new PhoneUnlockTool
            {
                Id= (int) UnlockTool.TUnlock,
                Name= "T-Unlock"
            },
            new PhoneUnlockTool
            {
                Id= (int) UnlockTool.SamKey,
                Name= "SamKey"
            },
             new PhoneUnlockTool
             {
                Id= (int) UnlockTool.GlobalUnlocker,
                Name= "Global Unlocker"
             }
        };
    }
}