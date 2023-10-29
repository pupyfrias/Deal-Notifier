using  Catalog.Domain.Entities;
using Enums = Catalog.Application.Enums;
namespace Catalog.Persistence.Seeds
{
    public static class ConditionSeed
    {
        public static List<Condition> Data { get; } = Enum.GetValues<Enums.Condition>()
                                                      .Select(e => new Condition { Id = (int)e, Name = e.ToString() })
                                                      .ToList();
    }
}