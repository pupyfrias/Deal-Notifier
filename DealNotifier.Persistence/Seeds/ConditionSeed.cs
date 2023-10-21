using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Persistence.Seeds
{
    public static class ConditionSeed
    {
        public static List<Condition> Data { get; } = Enum.GetValues<Enums.Condition>()
                                                      .Select(e => new Condition { Id = (int)e, Name = e.ToString() })
                                                      .ToList();
    }
}