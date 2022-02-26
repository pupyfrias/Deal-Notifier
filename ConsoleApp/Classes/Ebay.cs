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
        public Ebay(string url, ChromeOptions options)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Ebay");
            
            bool run = true;
            using (IWebDriver driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                }
                catch (WebDriverException e)
                {
                    WriteLogs($"ERROR: ---> {e.Message} | url:{url}", "ebay");
                    run = false;
                    driver.Quit();
                }

                int counter = 1;
                ArrayList arrayList = new ArrayList();
                ArrayList arrayListPreviuos = new ArrayList();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                while (run)
                {
                    try
                    {
                        By selector = By.CssSelector("li[class='s-item s-item--large s-item--bgcolored']");
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
                                string name = $"{RemoveSpecialCharacters(eName.Text)}";
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

                                Parallel.ForEach(conditionList, (data, state) =>
                                {
                                    if (name.ToLower().Contains(data))
                                    {
                                        condition = 0;
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

                                await SaveOrUpdate(save, name, link, image, price, condition, shop);
                            }
                            catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException | e is FormatException)
                            {
                                WriteLogs($"ERROR: ---> {e.Message}", "ebay");
                            }

                        });

                    }
                    catch (WebDriverTimeoutException)
                    {
                        ScreensShot(driver, $"Ebay-End {counter}", ebayLinkPositon);
                    }
                    catch (Exception)
                    {
                        ScreensShot(driver, $"Ebay-End {counter}", ebayLinkPositon);
                    }

                    Console.WriteLine($"\nEbay {counter} | {ebayLinkPositon}");
                    counter++;

                    try
                    {
                        string first = url.Substring(0, url.IndexOf("&_pgn=") + 6);
                        string page = url.Substring(url.IndexOf("&_pgn=") + 6);
                        string last = page.Substring(page.IndexOf("&"));
                        string link = first + counter + last;
                        var pagination = driver.FindElement(By.CssSelector("a[class='pagination__next icon-link']")).GetAttribute("href");

                        if (pagination.Contains("#"))
                        {
                            ScreensShot(driver, $"Ebay-End {counter}", ebayLinkPositon);
                            driver.Quit();
                            break;
                        }

                        driver.Url = link;
                        driver.Navigate();
                    }
                    catch (Exception e) when (e is WebDriverTimeoutException || e is WebDriverException)
                    {
                        ScreensShot(driver, $"Ebay-End {counter}", ebayLinkPositon);
                        driver.Quit();
                        break;
                    }

                }
                
            }

        }
    }
}
