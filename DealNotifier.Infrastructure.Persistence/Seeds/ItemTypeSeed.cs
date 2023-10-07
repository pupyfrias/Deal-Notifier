using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;


namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class ItemTypeSeed
    {
        public static List<ItemType> Data { get;} = new List<ItemType>
        {
            new ItemType
            {
                Id= (int) Enums.ItemType.Accessory,
                Name= Enums.ItemType.Accessory.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.Headphone,
                Name= Enums.ItemType.Headphone.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.Memory,
                Name= Enums.ItemType.Memory.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.Microphone,
                Name= Enums.ItemType.Microphone.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.Phone,
                Name= Enums.ItemType.Phone.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.Speaker,
                Name= Enums.ItemType.Speaker.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.Streaming,
                Name= Enums.ItemType.Streaming.ToString()
            },
            new ItemType
            {
                Id= (int) Enums.ItemType.TV,
                Name= Enums.ItemType.TV.ToString()
            }
        };
    }
}