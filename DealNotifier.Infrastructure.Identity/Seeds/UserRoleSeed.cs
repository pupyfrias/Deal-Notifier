using DealNotifier.Core.Application.Constants;
using Microsoft.AspNetCore.Identity;

namespace DealNotifier.Infrastructure.Identity.Seeds
{
    public static class UserRoleSeed
    {
        public static readonly IReadOnlyList<IdentityUserRole<string>> data = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                UserId = UserSeed.data.First(x => x.UserName =="superuser").Id,
                RoleId = RoleSeed.data.First(x=> x.Name == Role.SuperAdmin).Id
            },
             new IdentityUserRole<string>
            {
                UserId = UserSeed.data.First(x => x.UserName =="superuser").Id,
                RoleId = RoleSeed.data.First(x=> x.Name == Role.Basic).Id
            },
            new IdentityUserRole<string>
            {
                UserId = UserSeed.data.First(x => x.UserName =="basicuser").Id,
                RoleId = RoleSeed.data.First(x=> x.Name == Role.Basic).Id
            },
        };
    }
}