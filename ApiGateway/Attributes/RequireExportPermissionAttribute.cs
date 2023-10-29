namespace ApiGateway.Attributes
{
    public class RequireExportPermissionAttribute : RequirePermissionBaseAttribute
    {
        public RequireExportPermissionAttribute() : base("Export")
        {

        }
    }


}