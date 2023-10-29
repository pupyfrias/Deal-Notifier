namespace Identity.Persistence.Entity
{
    public class UserResourcePermission
    {
        public Permission Permission { get; set; }
        public int PermissionId { get; set; }
        public Resource Resource { get; set; }
        public int ResourceId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }

}