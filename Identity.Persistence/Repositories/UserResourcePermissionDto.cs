namespace Identity.Persistence.Repositories
{
    public class UserResourcePermissionDto
    {
        public string  PermissionName { get; set; }
        public int PermissionId { get; set; }
        public string ResourceName { get; set; }
        public int ResourceId { get; set; }
        public string UserId { get; set; }
    }
}