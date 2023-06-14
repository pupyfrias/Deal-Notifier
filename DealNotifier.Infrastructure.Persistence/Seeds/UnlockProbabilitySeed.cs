using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class UnlockProbabilitySeed
    {
        public static List<UnlockProbability> data { get; set; } = new List<UnlockProbability>
        {
            new UnlockProbability
            {
                Id= (int) Enums.UnlockProbability.None,
                Name= Enums.UnlockProbability.None.ToString()
            },
            new UnlockProbability
            {
                Id= (int) Enums.UnlockProbability.Low,
                Name= Enums.UnlockProbability.Low.ToString()
            },
            new UnlockProbability
            {
                Id= (int) Enums.UnlockProbability.Middle,
                Name= Enums.UnlockProbability.Middle.ToString()
            },
            new UnlockProbability
            {
                Id= (int) Enums.UnlockProbability.High,
                Name= Enums.UnlockProbability.High.ToString()
            }
        };
    }
}