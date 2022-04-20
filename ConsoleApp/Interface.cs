using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal interface IRun
    {
        Task Run(object[,] links, ChromeOptions options, ChromeDriverService service);
    }
}
