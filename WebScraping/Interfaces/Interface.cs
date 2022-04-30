using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;

namespace WebScraping
{
    internal interface IRun
    {
        Task Run(ChromeOptions options, ChromeDriverService service);
    }
}
