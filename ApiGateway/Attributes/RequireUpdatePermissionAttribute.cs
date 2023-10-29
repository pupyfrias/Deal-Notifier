namespace ApiGateway.Attributes
{
    public class RequireUpdatePermissionAttribute : RequirePermissionBaseAttribute
    {
        public RequireUpdatePermissionAttribute() : base("Update")
        {

        }
    }


}