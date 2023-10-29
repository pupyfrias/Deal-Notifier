using Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Persistence.Seeds
{
    public static class UserRoleSeed
    {
        public static List<IdentityUserRole<string>> Data { get; } = new() 
        {
            #region Admin

               new IdentityUserRole<string>
               {
                    UserId = UserSeed.Data.First(x => x.NormalizedUserName == "ADMIN").Id,
                    RoleId = RoleSeed.Data.First(x=> x.Name == Role.Admin).Id
               },
               new IdentityUserRole<string>
               {
                    UserId = UserSeed.Data.First(x => x.NormalizedUserName == "ADMIN").Id,
                    RoleId = RoleSeed.Data.First(x=> x.Name == Role.BasicUser).Id
               },

            #endregion Admin


            #region Basic User

            new IdentityUserRole<string>
            {
                UserId = UserSeed.Data.First(x => x.NormalizedUserName == "BASICUSER").Id,
                RoleId = RoleSeed.Data.First(x=> x.Name == Role.BasicUser).Id
            }

            #endregion Basic User
        };
    }
}