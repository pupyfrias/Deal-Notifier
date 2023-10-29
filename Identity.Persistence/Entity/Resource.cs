namespace Identity.Persistence.Entity
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserResourcePermission> UserResourcePermissions { get; set; }
    }

}