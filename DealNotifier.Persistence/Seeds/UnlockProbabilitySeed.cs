using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Persistence.Seeds
{
    public static class UnlockProbabilitySeed
    {
        public static List<UnlockProbability> Data { get; } = Enum.GetValues<Enums.UnlockProbability>()
                                                              .Select(e => new UnlockProbability { Id = (int)e, Name = e.ToString() })
                                                              .ToList();

    }
}