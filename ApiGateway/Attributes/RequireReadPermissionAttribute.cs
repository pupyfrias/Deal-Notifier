namespace ApiGateway.Attributes
{
    public class RequireReadPermissionAttribute : RequirePermissionBaseAttribute
    {
        public RequireReadPermissionAttribute() : base("Read")
        {
            
        }
    }


}