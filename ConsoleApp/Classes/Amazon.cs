using ConsoleApp.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Threading.Tasks;



namespace ConsoleApp
{
    internal class Amazon: Method , IRun
    {

        public async Task Run(object[,] links, ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Amazon");
            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                for (int i = 0; i < links.Length / 2; i++)
                {
                    int counter = 1;
                    bool run = true;

                    try
                    {
                        driver.Navigate().GoToUrl((string)links[i, 0]);
                    }
                    catch (WebDriverException e)
                    {
                        await WriteLogs($"BAD URL: ---> {e.Message} | url:{(string)links[i, 0]}", "Amazon");
                        run = false;
                        driver.Quit();
                    }

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                    string error = null;

                    while (run)
                    {
                        try
                        {
                            By selector = By.CssSelector("div[data-component-type='s-search-result']");

                            wait.Until(ExpectedConditions.ElementIsVisible(selector));
                            ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                            Parallel.ForEach(elements, async (element) =>
                            {
                                try
                                {
                                    IWebElement eName = element.FindElement(By.XPath(".//h2/a/span"));
                                    IWebElement eLink = element.FindElement(By.XPath(".//h2/a"));
                                    IWebElement eImage = element.FindElement(By.ClassName("s-image"));
                                    IWebElement ePriceWhole = element.FindElement(By.CssSelector("span[class='a-price-whole']"));
                                    IWebElement ePriceFraction = element.FindElement(By.ClassName("a-price-fraction"));
                                    string name = RemoveSpecialCharacters(eName.Text);
                                    string link = eLink.GetAttribute("href");
                                    link = link.Substring(0, link.IndexOf("/ref"));
                                    string image = eImage.GetAttribute("src").Replace("218", "320");
                                    decimal price = decimal.Parse(ePriceWhole.Text.Replace(",", "") + "." + ePriceFraction.Text);
                                    int condition = 1;
                                    bool save = true;
                                    string shop = "Amazon";
                                    int type = (int)links[i, 1];

                                    error = link;

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
                                catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                                {
                                    await WriteLogs($"ERROR: --->  | URL: {i} | {e.Message}", "Amazon");
                                }
                                catch (EntityCommandExecutionException)
                                {
                                    Console.WriteLine(error);
                                }

                            });
                        }
                        catch (WebDriverTimeoutException)
                        {
                            ScreensShot(driver, $"Amazon {i}", counter);
                        }


                        try
                        {
                            wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='s-pagination-item s-pagination-next s-pagination-button s-pagination-separator'"))).Click();

                        }
                        catch (WebDriverTimeoutException)
                        {
                            ScreensShot(driver, $"Amazon-End {i}", counter);
                            break;
                        }

                        Console.WriteLine($"\nAmazon {i} | {counter} ");
                        counter++;

                    }

                }
                driver.Quit();

            }
        }
    }
}
