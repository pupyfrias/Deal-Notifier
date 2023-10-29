using Microsoft.AspNetCore.Identity;

namespace Identity.Persistence.Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<UserResourcePermission> UserResourcePermissions { get; set; }
    }

}