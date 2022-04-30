using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using WebScraping.Classes;

namespace WebScraping
{
    internal class Ebay : Method, IRun
    {
        private object[,] links =
         {
           {"https://www.ebay.com/sch/i.html?_dcat=9355&_fsrp=1&_blrs=recall_filtering&_from=R40&LH_TitleDesc=0&LH_ItemCondition=1000&Brand=Samsung%7CApple%7CMotorola%7CAlcatel%7CXiaomi%7CHuawei%7CLG&_nkw=phones&_sacat=9355&Network=AT%2526T%7CCricket%2520Wireless%7CSprint%7CT%252DMobile%7CUnlocked%7CBoom%2520Mobile%7CBoost%2520Mobile%7CMetro%7CVerizon%7CUS%2520Mobile%7CU%252ES%252E%2520Cellular%7CVirgin%2520Mobile&LH_BIN=1&Storage%2520Capacity=32%2520GB%7C64%2520GB%7C256%2520GB%7C512%2520GB%7C128%2520GB&_sop=15&_udhi=120&rt=nc&_stpos=19720&_sadis=2000&LH_PrefLoc=99&_fspt=1&_ipg=240",1 },
           {"https://www.ebay.com/sch/i.html?_dcat=11071&_fsrp=1&rt=nc&_from=R40&LH_PrefLoc=99&LH_ItemCondition=1000&_stpos=19720&_nkw=tv&_sacat=32852&LH_BIN=1&_fspt=1&Screen%2520Size=60%252D69%2520in%7C20%252D29%2520in%7C40%252D49%2520in%7C30%252D39%2520in%7C50%252D59%2520in%7C70%252D80%2520in&_udhi=200&_sadis=2000&_ipg=240" ,2}
          };

        public async Task Run(ChromeOptions options, ChromeDriverService service)
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
                        await WriteLogs($"eBay: ---> {e.Message.Trim()} | URL:{(string)links[i, 0]}");
                        driver.Close();
                        run = false;

                    }

                    int counter = 1;
                    ArrayList arrayList = new ArrayList();
                    ArrayList arrayListPreviuos = new ArrayList();
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                    while (run)
                    {
                        try
                        {
                            By selector = By.CssSelector("div[class=\"s-item__wrapper clearfix\"]");
                            wait.Until(ExpectedConditions.ElementToBeClickable(selector));
                            Thread.Sleep(2000);

                            ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                            Parallel.ForEach(elements, async (element, stateMain) =>
                            {
                                try
                                {
                                    IWebElement eName = element.FindElement(By.ClassName("s-item__title"));
                                    IWebElement eLink = element.FindElement(By.ClassName("s-item__link"));
                                    IWebElement eImage = element.FindElement(By.ClassName("s-item__image-img"));
                                    IWebElement ePriceWhole = element.FindElement(By.ClassName("s-item__price"));
                                    string name = RemoveSpecialCharacters((eName.Text).Replace("NEW LISTING", ""));
                                    string link = eLink.GetAttribute("href");
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

                                    Parallel.ForEach(conditionList, (data, state) =>
                                    {
                                        if (name.ToLower().Contains(data))
                                        {
                                            condition = 2;
                                            state.Break();
                                        }
                                    });

                                    Parallel.ForEach(filterList, (data, state) =>
                                    {
                                        if (name.ToLower().Contains(data))
                                        {
                                            save = false;
                                            state.Break();
                                        }
                                    });

                                    if (blackList.Contains(link))
                                    {
                                        save = false;
                                    }

                                    await SaveOrUpdate(save, name, link, image, price, condition, shop, type);
                                }
                                catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException | e is FormatException |
                                                          e is WebDriverException)
                                {
                                    await WriteLogs($"eBay: --->  | URL: {i} | {e.Message.Trim()}");
                                }


                            });

                        }
                        catch (WebDriverTimeoutException e)
                        {
                            await WriteLogs($"eBay: --->  | URL: {i} | {e}");
                        }


                        Console.WriteLine($"\nEbay {i} | {counter} ");
                        counter++;

                        try
                        {

                            var currentLink = driver.FindElement(By.CssSelector("a[class=\"pagination__next icon-link\"]")).GetAttribute("href");

                            if (currentLink == driver.Url)
                            {
                                await ScreensShot(driver, "Ebay", i, counter);
                                break;
                            }
                            else
                            {
                                wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class=\"pagination__next icon-link\"]"))).Click();
                            }

                        }
                        catch (Exception e) when (e is WebDriverTimeoutException || e is WebDriverException)
                        {
                            await ScreensShot(driver, "Ebay", i, counter);
                            break;
                        }

                    }
                }

                driver.Quit();

            }
        }
    }
}
