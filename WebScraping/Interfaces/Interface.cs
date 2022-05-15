using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;

namespace WebScraping
{
    internal interface IRun
    {
        void Run(ChromeOptions options, ChromeDriverService service);
    }
}
