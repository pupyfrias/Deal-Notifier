using Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Persistence.Seeds
{
    public static class RoleSeed
    {
        public static List<IdentityRole<string>> Data { get; } = new List<IdentityRole<string>>
        {
            new IdentityRole
            {
                Name = Role.Admin,
                NormalizedName = Role.Admin.ToUpper()
            },
            new IdentityRole
            {
                Name = Role.BasicUser,
                NormalizedName = Role.BasicUser.ToUpper()
            }
        };
    }
}