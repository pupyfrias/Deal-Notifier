using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class ConditionSeed
    {
        public static List<Condition> data { get; set; } = new List<Condition>
        {
            new Condition
            {
                Id= (int) Enums.Condition.New,
                Name= Enums.Condition.New.ToString()
            },
            new Condition
            {
                Id= (int) Enums.Condition.Used,
                Name= Enums.Condition.Used.ToString()
            }
        };
    }
}