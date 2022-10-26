using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using WebScraping.Core.Application.Emuns;
using WebScraping.Core.Application.Extensions;
using WebScraping.Core.Application.Utils;
using WebScraping.Core.Domain.Entities;
using WebScraping.Intrastructure.Persistence.Services;
using Type = WebScraping.Core.Application.Emuns.Type;
using Condition = WebScraping.Core.Application.Emuns.Condition;
using Shop = WebScraping.Core.Application.Emuns.Shop;
using Status = WebScraping.Core.Application.Emuns.Status;

namespace WebScraping.Intrastructure.Persistence.Models
{
    public class Ebay : ItemService
    {
        private static ILogger _logger;
        private static string error;
        private static object[,] links =
         {
           {"https://www.ebay.com/sch/i.html?_fsrp=1&_udlo=30&_blrs=recall_filtering&_from=R40&LH_TitleDesc=0&LH_PrefLoc=98&LH_ItemCondition=1000&Brand=Samsung%7CApple%7CMotorola%7CAlcatel%7CXiaomi%7CHuawei%7CLG&_stpos=19720&_nkw=phones&_sacat=9355&LH_BIN=1&Storage%2520Capacity=32%2520GB%7C64%2520GB%7C256%2520GB%7C512%2520GB%7C128%2520GB&_sop=15&_fspt=1&_udhi=120&rt=nc&Operating%2520System=Android&_dcat=9355&_ipg=240&_pgn=1", Type.Phone }

        };
        public static void Run()
        {
            _logger = LogConsole.CreateLogger<Ebay>();
            string option = $@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Ebay";
            bool run = true;

            using (IWebDriver driver = Selenium.CreateChromeDriver(option))
            {
                for (int i = 0; i < links.Length / 2; i++)
                {
                    try
                    {
                        driver.Navigate().GoToUrl((string)links[i, 0]);
                    }
                    catch (WebDriverException e)
                    {
                        _logger.LogError($"{e} | URL:{(string)links[i, 0]}");
                        driver.Close();
                        run = false;

                    }

                    int counter = 1;
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    HashSet<Item> itemList = new HashSet<Item>();

                    while (run)
                    {
                        try
                        {
                            By selector = By.CssSelector("div[class=\"s-item__wrapper clearfix\"]");
                            wait.Until(ExpectedConditions.ElementIsVisible(selector));
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("document.getElementsByClassName('srp-rail__right')[0]?.remove()");
                            js.ExecuteScript("document.getElementsByClassName('srp-rail__left')[0]?.remove()");
                            js.ExecuteScript("document.getElementsByClassName('srp-main-below-river')[0]?.remove()");
                            js.ExecuteScript("document.getElementsByClassName('s-answer-region s-answer-region-center-top')[0]?.remove()");

                            ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                            Parallel.ForEach(elements, async (element, stateMain) =>
                            {
                                string link = null;

                                try
                                {
                                    IWebElement eName = element.FindElement(By.ClassName("s-item__title"));
                                    IWebElement eLink = element.FindElement(By.ClassName("s-item__link"));
                                    IWebElement eImage = element.FindElement(By.ClassName("s-item__image-img"));
                                    IWebElement ePriceWhole = element.FindElement(By.ClassName("s-item__price"));

                                    link = Uri.UnescapeDataString(eLink.GetAttribute("href"));
                                    link = link.Substring(0, link.IndexOf("?"));
                                    string image = eImage.GetAttribute("src").Replace("225", "425");
                                    if (image.ToLower().Contains("ebaystatic.com"))
                                        image = eImage.GetAttribute("data-src");
                                    string priceBruto = ePriceWhole.Text.Replace("$", "");
                                    if (priceBruto.ToLower().Contains("to"))
                                        priceBruto = priceBruto.Substring(0, priceBruto.IndexOf("to"));


                                    Item item = new Item();
                                    item.Name = eName.Text.Replace("NEW LISTING", "").RemoveSpecialCharacters();
                                    item.Link = link;
                                    item.Image = image;
                                    item.Price = decimal.Parse(priceBruto);
                                    item.ConditionId = (int)Condition.New;
                                    item.ShopId = (int)Shop.eBay;
                                    item.TypeId = (int)links[i, 1];
                                    item.StatusId = (int)Status.InStock;

                                    if (await CanBeSave(item))
                                    {
                                        SetCondition(ref item);
                                        itemList.Add(item);
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (link != "https://ebay.com/itm/123456")
                                    {
                                        error = $"URL: {link} | {e.Message}";
                                        _logger.LogWarning(error);
                                        LogFile.Write<Ebay>(error);
                                    }

                                }

                            });
                        }
                        catch (WebDriverTimeoutException e)
                        {
                            error = $"URL: {i} | {e.Message}";
                            _logger.LogWarning(error);
                            LogFile.Write<Ebay>(error);
                        }


                        _logger.LogInformation($"{i}\t| {counter}\t| {itemList.Count}");
                        counter++;

                        try
                        {
                            var currentLink = driver.FindElement(By.CssSelector("a[class=\"pagination__next icon-link\"]")).GetAttribute("href");

                            if (currentLink == driver.Url)
                            {
                                string shop = "Ebay";
                                string by = "getElementsByClassName('s-pagination')[0]";
                                ScreenShot.TakeAtBottom(driver, ref shop, ref i, ref counter, ref by);
                                break;
                            }
                            else
                            {
                                wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class=\"pagination__next icon-link\"]"))).Click();
                            }

                        }
                        catch (Exception e) when (e is WebDriverTimeoutException || e is WebDriverException)
                        {
                            string shop = "Ebay";
                            string by = "getElementsByClassName('s-pagination')[0]";
                            ScreenShot.TakeAtBottom(driver, ref shop, ref i, ref counter, ref by);
                            break;
                        }

                    }


                    Save(ref itemList);
                }

                driver.Quit();

            }
        }
    }
}
