using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebScraping.Classes;

namespace WebScraping
{
    internal class Ebay : Method, IRun
    {
        private object[,] links =
         {
           {"https://www.ebay.com/sch/i.html?_fsrp=1&_udlo=30&_blrs=recall_filtering&_from=R40&LH_TitleDesc=0&LH_PrefLoc=98&LH_ItemCondition=1000&_ipg=60&Brand=Samsung%7CApple%7CMotorola%7CAlcatel%7CXiaomi%7CHuawei%7CLG&_stpos=19720&_nkw=phones&_sacat=9355&LH_BIN=1&Storage%2520Capacity=32%2520GB%7C64%2520GB%7C256%2520GB%7C512%2520GB%7C128%2520GB&_sop=15&_fspt=1&_udhi=120&rt=nc&Operating%2520System=Android&_dcat=9355&_pgn=1",1 },
           {"https://www.ebay.com/sch/i.html?_dcat=11071&_fsrp=1&_from=R40&LH_PrefLoc=99&LH_ItemCondition=1000&_stpos=19720&_nkw=tv&_sacat=32852&LH_BIN=1&_fspt=1&Screen%2520Size=60%252D69%2520in%7C20%252D29%2520in%7C40%252D49%2520in%7C30%252D39%2520in%7C50%252D59%2520in%7C70%252D80%2520in&_udhi=200&_sadis=2000&_ipg=60&rt=nc&_udlo=50" ,2}
          };

        public void Run(ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Ebay");
            bool run = true;
            
            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                for (int i = 0; i < links.Length / 2; i++)
                {
                    try
                    {
                        driver.Navigate().GoToUrl((string)links[i, 0]);
                    }
                    catch (WebDriverException e)
                    {
                        WriteLogs($"eBay Close: ---> {e} | URL:{(string)links[i, 0]}");
                        driver.Close();
                        run = false;

                    }

                    int counter = 1;
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

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
                                    link = eLink.GetAttribute("href");
                                    string name = eName.Text.Replace("NEW LISTING", "");
                                    RemoveSpecialCharacters(ref name);
                                    link = link.Substring(0, link.IndexOf("?"));
                                    string image = eImage.GetAttribute("src").Replace("225", "425");
                                    if (image.ToLower().Contains("ebaystatic.com")) { image = eImage.GetAttribute("data-src"); }
                                    string priceBruto = ePriceWhole.Text.Replace("$", "");
                                    if (priceBruto.ToLower().Contains("to")) { priceBruto = priceBruto.Substring(0, priceBruto.IndexOf("to")); }

                                    decimal price = decimal.Parse(priceBruto);
                                    int condition = 1;
                                    bool save = true;
                                    int shop = 2;
                                    int type = (int)links[i, 1];


                                    if (blackList.Contains(link))
                                    {
                                        save = false;
                                    }
                                    else
                                    {
                                        var task1 = Task.Run(() =>
                                        {
                                            Parallel.ForEach(conditionList, (data, state) =>
                                            {
                                                if (name.ToLower().Contains(data))
                                                {
                                                    condition = 2;
                                                    state.Break();
                                                }
                                            });
                                        });

                                        var task2 = Task.Run(() =>
                                        {
                                            Parallel.ForEach(filterList, (data, state) =>
                                            {
                                                if (name.ToLower().Contains(data))
                                                {
                                                    save = false;
                                                    state.Break();
                                                }
                                            });
                                        });

                                        await Task.WhenAll(task1, task2);
                                    }
                                    SaveOrUpdate(ref save, ref name, link, ref image,ref price,ref condition, ref shop, ref type);
                                }
                                catch (Exception e) 
                                {
                                    if(link!= "https://ebay.com/itm/123456")
                                    WriteLogs($"\neBay: --->  | URL: {link} | {e}");
                                }


                            });

                        }
                        catch (WebDriverTimeoutException e)
                        {
                            WriteLogs($"\neBay: --->  | URL: {i} | {e}");
                        }

                        Console.WriteLine($"\nEbay   {i}\t{counter} ");
                        counter++;

                        try
                        {
                            var currentLink = driver.FindElement(By.CssSelector("a[class=\"pagination__next icon-link\"]")).GetAttribute("href");

                            if (currentLink == driver.Url)
                            {
                                string shop = "Ebay";
                                string by = "getElementsByClassName('s-pagination')[0]";
                                ScreensShot(driver, ref shop, ref i, ref counter, ref by);
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
                            ScreensShot(driver, ref shop, ref i, ref counter, ref by);
                            break;
                        }

                    }
                }

                driver.Quit();

            }
        }
    }
}
