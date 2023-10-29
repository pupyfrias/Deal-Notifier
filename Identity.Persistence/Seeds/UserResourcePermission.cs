using Identity.Persistence.Entity;
using Permission = Identity.Application.Enums.Permission;

namespace Identity.Persistence.Seeds
{
    public static class UserResourcePermissionSeed
    {


        public static List<UserResourcePermission> Data { get; } = GenerateData();

        private static List<UserResourcePermission> GenerateData()
        {
            var list = new List<UserResourcePermission>();
            #region Getting ResourcesId

            int authResourceId = ResourceSeed.Data.First(x => x.Name == "Auth").Id;
            int userResourceId = ResourceSeed.Data.First(x => x.Name == "Users").Id;
            int roleResourceId = ResourceSeed.Data.First(x => x.Name == "Roles").Id;
            int itemResourceId = ResourceSeed.Data.First(x => x.Name == "Items").Id;

            #endregion Getting ResourcesId

            #region Getting UsersId

            string adminId = UserSeed.Data.First(x => x.NormalizedUserName == "ADMIN").Id;
            string basicUserId = UserSeed.Data.First(x => x.NormalizedUserName == "BASICUSER").Id;

            #endregion Getting UsersId

            #region Admin
            foreach (var permission in Enum.GetValues<Permission>())
            {

                var adminUserAuthResourcePermission = new UserResourcePermission
                {
                    PermissionId = (int)permission,
                    ResourceId = authResourceId,
                    UserId = adminId
                };

                list.Add(adminUserAuthResourcePermission);

                var adminUserUserResourcePermission = new UserResourcePermission
                {
                    PermissionId = (int)permission,
                    ResourceId = userResourceId,
                    UserId = adminId
                };

                list.Add(adminUserUserResourcePermission);

                var adminUserRoleResourcePermission = new UserResourcePermission
                {
                    PermissionId = (int)permission,
                    ResourceId = roleResourceId,
                    UserId = adminId
                };

                list.Add(adminUserRoleResourcePermission);

                var adminUserItemResourcePermission = new UserResourcePermission
                {
                    PermissionId = (int)permission,
                    ResourceId = itemResourceId,
                    UserId = adminId
                };

                list.Add(adminUserItemResourcePermission);

            }
            #endregion Admin

            #region BasicUser
            var basicUserItemResourcePermissionRead = new UserResourcePermission
            {
                PermissionId = (int)Permission.Read,
                ResourceId = authResourceId,
                UserId = basicUserId
            };

            list.Add(basicUserItemResourcePermissionRead);
            #endregion BasicUser
            return list;
        }
    }
}