using Enums = WebScraping.Core.Application.Enums;
using Type = WebScraping.Core.Domain.Entities.Type;

namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public static class TypeSeed
    {
        public static List<Type> data { get; set; } = new List<Type>
        {
            new Type
            {
                Id= (int) Enums.Type.Accessory,
                Name= Enums.Type.Accessory.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.Headphone,
                Name= Enums.Type.Headphone.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.Memory,
                Name= Enums.Type.Memory.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.Microphone,
                Name= Enums.Type.Microphone.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.Phone,
                Name= Enums.Type.Phone.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.Speaker,
                Name= Enums.Type.Speaker.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.Streaming,
                Name= Enums.Type.Streaming.ToString()
            },
            new Type
            {
                Id= (int) Enums.Type.TV,
                Name= Enums.Type.TV.ToString()
            }
        };
    }
}