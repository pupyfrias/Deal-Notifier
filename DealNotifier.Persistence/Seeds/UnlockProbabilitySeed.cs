
using Catalog.Domain.Entities;
using Enums =  Catalog.Application.Enums;

namespace Catalog.Persistence.Seeds
{
    public static class UnlockProbabilitySeed
    {
        public static List<UnlockProbability> Data { get; } 
            = Enum.GetValues<Enums.UnlockProbability>()
            .Select(e => new UnlockProbability { Id = (int)e, Name = e.ToString() })
            .ToList();

    }
}