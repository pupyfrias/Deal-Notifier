using Identity.Persistence.Entity;

namespace Identity.Persistence.Seeds
{
    public static class PermissionSeed
    {
        public static List<Permission> Data { get; }
            = Enum.GetValues<Application.Enums.Permission>()
            .Select(e => new Permission { Id = (int)e, Name = e.ToString() })
            .ToList();
    }

} 