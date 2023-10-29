namespace ApiGateway.Attributes
{
    public class RequireCreatePermissionAttribute : RequirePermissionBaseAttribute
    {
        public RequireCreatePermissionAttribute() : base("Create")
        {

        }
    }


}