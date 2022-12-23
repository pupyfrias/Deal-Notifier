using Emuns = WebScraping.Core.Application.Emuns;
using Type = WebScraping.Core.Domain.Entities.Type;

namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public class TypeSeed
    {
        public static List<Type> data = new List<Type>
        {
            new Type
            {
                Id= (int) Emuns.Type.Accessory,
                Name= Emuns.Type.Accessory.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.Headphone,
                Name= Emuns.Type.Headphone.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.Memory,
                Name= Emuns.Type.Memory.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.Microphone,
                Name= Emuns.Type.Microphone.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.Phone,
                Name= Emuns.Type.Phone.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.Speaker,
                Name= Emuns.Type.Speaker.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.Streaming,
                Name= Emuns.Type.Streaming.ToString()
            },
            new Type
            {
                Id= (int) Emuns.Type.TV,
                Name= Emuns.Type.TV.ToString()
            }
        };
    }
}