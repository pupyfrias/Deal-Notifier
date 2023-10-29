
namespace ApiGateway.Attributes
{
    public class RequireDeletePermissionAttribute : RequirePermissionBaseAttribute
    {
        public RequireDeletePermissionAttribute() : base("Delete")
        {

        }

    }


}