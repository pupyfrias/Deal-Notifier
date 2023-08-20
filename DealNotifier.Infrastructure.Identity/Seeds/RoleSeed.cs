using Microsoft.AspNetCore.Identity;
using DealNotifier.Core.Application.Constants;

namespace DealNotifier.Infrastructure.Identity.Seeds
{
    public static class RoleSeed
    {
        public readonly static IReadOnlyList<IdentityRole<string>> data = new List<IdentityRole<string>>
        {
            new IdentityRole
            {
                Name = Role.SuperAdmin,
                NormalizedName = Role.SuperAdmin.ToUpper()
            },
            new IdentityRole
            {
                Name = Role.Admin,
                NormalizedName = Role.Admin.ToUpper()
            },
            new IdentityRole
            {
                Name = Role.Moderator,
                NormalizedName = Role.Moderator.ToUpper()
            },
            new IdentityRole
            {
                Name = Role.Basic,
                NormalizedName = Role.Basic.ToUpper()
            }
        };
    }
}