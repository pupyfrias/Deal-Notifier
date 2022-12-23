using Microsoft.AspNetCore.OData;

namespace WebScraping.Core.Application.SetupOptions
{
    public static class OData
    {
        public static Action<ODataOptions> Options = options =>
        {
            options.Select()
                .Filter()
                .OrderBy();
        };
    }
}