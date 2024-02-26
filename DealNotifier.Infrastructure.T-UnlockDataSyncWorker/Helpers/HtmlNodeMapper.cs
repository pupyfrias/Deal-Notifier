using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using HtmlAgilityPack;
using System.Xml.Linq;

namespace WorkerService.T_Unlock_WebScraping.Helpers
{
    public static class HtmlNodeMapper
    {
        public static HtmlNodeCollection MapStringToHtmlNodeCollection(string pageHtml)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageHtml);
            return htmlDocument.DocumentNode.SelectNodes("//div[@class='MyCRD']");
        }


        public static UnlockedPhoneDetailsDto MapHtmlNodeToPhoneDetails(HtmlNode htmlNode)
        {
            var thNode = htmlNode.SelectSingleNode("//th");
            var tdNode = htmlNode.SelectSingleNode("//tr/td");
            
            var h7s = thNode.SelectNodes(".//h7");
            var h4 = tdNode.SelectSingleNode(".//h4");


            string modelNumber = h7s[0].InnerText;
            string modelName = h7s[1].InnerText;
            string carrierList = h4.InnerText;


            var phoneDetailsTUnlock = new UnlockedPhoneDetailsDto
            {
                Carriers = carrierList,
                ModelName = modelName,
                ModelNumber = modelNumber

            };

            return phoneDetailsTUnlock;
        }
    }
}
