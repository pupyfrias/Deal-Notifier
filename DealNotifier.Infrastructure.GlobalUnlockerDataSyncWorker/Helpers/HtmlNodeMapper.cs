

using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using HtmlAgilityPack;

namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Helpers
{
    public static class HtmlNodeMapper
    {
        public static HtmlNodeCollection MapStringToHtmlNodeCollection(string pageHtml)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageHtml);
            return htmlDocument.DocumentNode.SelectNodes("//*[@id=\"models\"]/tbody/tr");
        }


        public static UnlockedPhoneDetailsDto? MapHtmlNodeToPhoneDetails(HtmlNode htmlNode)
        {

            var tds = htmlNode.SelectNodes("td");

            string service = tds[2].InnerText;
            if (!service.Contains("unlock", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string modelName = tds[0].InnerText;
            string modelNumber = tds[1].InnerText;
            string carrierList = tds[4].InnerText;


            var phoneDetailsGlobalUnlocker = new UnlockedPhoneDetailsDto
            {
                Carriers = carrierList,
                ModelName = modelName,
                ModelNumber = modelNumber

            };

            return phoneDetailsGlobalUnlocker;
        }
    }
}
