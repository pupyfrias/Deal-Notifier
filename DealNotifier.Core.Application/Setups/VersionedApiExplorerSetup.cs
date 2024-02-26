using Asp.Versioning.ApiExplorer;

namespace DealNotifier.Core.Application.Setups
{
    public static class VersionedApiExplorerSetup
    {
        public readonly static Action<ApiExplorerOptions> Configure = options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        };
} }
