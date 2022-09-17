using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using WebScraping.Classes;
using WebScraping.Emuns;
using WebScraping.Models;

namespace WebScraping
{
    public class TheStore : Method, IRun
    {
        private static ILogger _logger;
        private object[,] links = { { "https://thestore.com/c/refurbished-cell-phones-58?condition=Brand%20New&showMore=0", Emuns.Type.Phone } };

        public void Run()
        {
            _logger = CreateLogger<TheStore>();
            string option = $@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\The Store";

            using (IWebDriver driver = CreateChromeDriver(option))
            {
                driver.Navigate().GoToUrl((string)links[0, 0]);
                int counter = 1;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                List<Item> itemsList = new List<Item>();

                while (true)
                {
                    try
                    {
                        By selector = By.CssSelector("div[class='product-tile']");
                        wait.Until(ExpectedConditions.ElementIsVisible(selector));

                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("document.getElementsByClassName('acsb-trigger acsb-bg-lead acsb-trigger-size-medium acsb-trigger-position-x-right acsb-trigger-position-y-bottom acsb-ready')[0]?.remove()");
                        js.ExecuteScript("document.getElementById('cc-button')?.remove()");
                        js.ExecuteScript("document.getElementsByClassName('hello-bar')[0]?.remove()");
                        js.ExecuteScript($"document.getElementsByClassName('pagination is-centered')[0]?.scrollIntoView();");
                        Thread.Sleep(2000);

                        ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                        Parallel.ForEach(elements, async (element) =>
                        {
                            try
                            {
                                IWebElement eName = element.FindElement(By.ClassName("product-tile__name"));
                                IWebElement eLink = element.FindElement(By.ClassName("product-tile__link"));
                                IWebElement eImage = element.FindElement(By.CssSelector("img[lazy='loaded'], img[lazy='loading']"));
                                IWebElement ePriceWhole = element.FindElement(By.ClassName("amount"));
                                IWebElement conditionName = element.FindElement(By.ClassName("product-tile__condition"));

                                var item = new Item();
                                item.Name = RemoveSpecialCharacters($"{eName.Text} {conditionName.Text}");
                                item.Link = Uri.UnescapeDataString(eLink.GetAttribute("href"));
                                item.Image = "https://thestore.com" + eImage.GetAttribute("data-src").Replace("height=300", "height=400");
                                item.Price = decimal.Parse(ePriceWhole.Text.Replace("$", ""));
                                item.Shop = (int)Shop.TheStore;
                                item.Type = (int)links[0, 1];
                                item.Condition = (int)Condition.New;
                                item.Save = true;

                                if (blackList.Contains(item.Link))
                                {
                                    item.Save = false;
                                }
                                else
                                {
                                    var task1 = Task.Run(() =>
                                    {
                                        Parallel.ForEach(conditionList, (data, state) =>
                                        {
                                            if (item.Name.ToLower().Contains(data))
                                            {
                                                item.Condition = (int)Condition.Used;
                                                state.Break();
                                            }
                                        });
                                    });

                                    var task2 = Task.Run(() =>
                                    {
                                        Parallel.ForEach(filterList, (data, state) =>
                                        {
                                            if (item.Name.ToLower().Contains(data))
                                            {
                                                item.Save = false;
                                                state.Break();
                                            }
                                        });
                                    });

                                    await Task.WhenAll(task1, task2);
                                }

                                if(item.Save)
                                    itemsList.Add(item);
                                
                            }
                            catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                            {
                                _logger.LogWarning(e.ToString());
                            }

                        });
                    }
                    catch (Exception e)
                    {
                        string shop = "The Store";
                        int link = 0;
                        _logger.LogError(e.ToString());
                        ScreensShot(driver, ref shop, ref link,ref counter);
                    }

                    _logger.LogInformation($"0\t| {counter}\t| {itemsList.Count}");
                    counter++;
                    try
                    { 
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[class='round-button round-button__next']"))).Click();
                    }

                    catch (WebDriverTimeoutException)
                    {
                        string shop = "The Store";
                        string by = "getElementsByClassName('pagination is-centered')[0]";
                        int link=0;
                      
                        ScreensShot(driver,ref shop, ref link,ref counter,ref by );
                        driver.Quit();
                        break;
                    }

                }

                Save(ref itemsList);
            }
        }

    }
}
