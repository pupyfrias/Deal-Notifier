using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiGateway.Setups
{
    public static class VersionedApiExplorerSetup
    {
        public readonly static Action<ApiExplorerOptions> Configure = options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        };
    }
}
