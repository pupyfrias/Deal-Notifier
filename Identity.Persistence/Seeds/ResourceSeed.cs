using Identity.Persistence.Entity;

namespace Identity.Persistence.Seeds
{
    public static class ResourceSeed
    {
        public static List<Resource> Data { get; } = new List<Resource>
        {
            new Resource {Id= 1, Name = "Auth"},
            new Resource {Id= 2,  Name = "Users"},
            new Resource {Id= 3,  Name = "Roles"},
            new Resource {Id= 4,  Name = "Items"}
        };
    }
}