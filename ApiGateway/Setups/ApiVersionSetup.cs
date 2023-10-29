using Asp.Versioning;

namespace ApiGateway.Setups
{
    public static class ApiVersionSetup
    {
        public static readonly Action<ApiVersioningOptions> Configure = options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

        };
    }
}