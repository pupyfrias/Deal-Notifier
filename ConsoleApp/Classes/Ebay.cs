using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Ebay: Program
    {
        public Ebay(object[,] links, ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Ebay");
            
            bool run = true;
            using (IWebDriver driver = new ChromeDriver(service, options))
            {


                for(int i = 0; i < links.Length/2; i++)
                {

                    try
                    {
                        driver.Navigate().GoToUrl((string) links[i,0]);
                    }
                    catch (WebDriverException e)
                    {
                        WriteLogs($"ERROR: ---> {e.Message} | url:{(string)links[i, 0]}", "ebay");
                        run = false;
                        driver.Quit();
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
                                    string image = eImage.GetAttribute("src");
                                    if (image.ToLower().Contains("ebaystatic.com")) { image = eImage.GetAttribute("data-src"); }
                                    string priceBruto = ePriceWhole.Text.Replace("$", "");
                                    if (priceBruto.ToLower().Contains("to")) { priceBruto = priceBruto.Substring(0, priceBruto.IndexOf("to")); }

                                    decimal price = decimal.Parse(priceBruto);
                                    int condition = 1;
                                    bool save = true;
                                    string shop = "Ebay";
                                    int type =(int) links[i, 1];

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
                                catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException | e is FormatException)
                                {
                                    WriteLogs($"ERROR: --->  | {e.Message}", $"ebay {i}");
                                }

                            });

                        }
                        catch (WebDriverTimeoutException)
                        {
                            ScreensShot(driver, $"Ebay-End {i}", counter);
                        }
                        /*catch (Exception)
                        {
                           ScreensShot(driver, $"Ebay-End {i}", counter );
                        }*/

                        Console.WriteLine($"\nEbay {i} | {counter} ");
                        counter++;

                        try
                        {

                            var currentLink = driver.FindElement(By.CssSelector("a[class=\"pagination__next icon-link\"]")).GetAttribute("href");

                            if (currentLink == driver.Url)
                            {
                                ScreensShot(driver, $"Ebay-End {i}", counter);
                                break;
                            }
                            else
                            {
                                wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class=\"pagination__next icon-link\"]"))).Click();
                            }

                        }
                        catch (Exception e) when (e is WebDriverTimeoutException || e is WebDriverException)
                        {
                            ScreensShot(driver, $"Ebay-End {i}", counter);
                            break;
                        }

                    }
                }
                driver.Quit();

            }

        }
    }
}
