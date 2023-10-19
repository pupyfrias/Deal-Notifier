using HtmlAgilityPack;
using WorkerService.T_Unlock_WebScraping.ViewModels;

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


        public static PhoneDetailsTUnlock MapHtmlNodeToPhoneDetails(HtmlNode htmlNode)
        {
            var thead = htmlNode.SelectSingleNode(".//thead");
            var h7List = thead.Descendants("h7").ToList();
            var h4 = htmlNode.SelectSingleNode(".//h4");

            string modelNumber = h7List[0].InnerText;
            string modelName = h7List[1].InnerText;
            string carrierList = h4.InnerText;


            var phoneDetailsTUnlock = new PhoneDetailsTUnlock
            {
                Carriers = carrierList,
                ModelName = modelName,
                ModelNumber = modelNumber

            };

            return phoneDetailsTUnlock;
        }
    }
}
