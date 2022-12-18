using WebScraping.Core.Domain.Entities;
using Emuns = WebScraping.Core.Application.Emuns;
namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public class ConditionSeed
    {
        public static List<Condition> data = new List<Condition>
        {
            new Condition
            {
                Id= (int) Emuns.Condition.New,
                Name= Emuns.Condition.New.ToString()
            },
            new Condition
            {
                Id= (int) Emuns.Condition.Used,
                Name= Emuns.Condition.Used.ToString() 
            }

        };  
    }
}
