using DealNotifier.Core.Application.Enums;

namespace WorkerService.T_Unlock_WebScraping.Helpers
{
    static class BrandHelper
    {
        public static Brand GetBrandByName(string path)
        {
            Dictionary<string, Brand> keyValues = new Dictionary<string, Brand>
            {
                { "samsung", Brand.Samsung },
                { "motorola", Brand.Motorola },
                { "lg", Brand.LG },
            };

            return keyValues.TryGetValue(path, out Brand brand) ? brand : Brand.Unknown;
        }
    }
}
