using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using WebScraping.Classes;

namespace WebScraping
{
    internal class TheStore : Method, IRun
    {
        private object[,] links = { { "https://thestore.com/c/refurbished-cell-phones-58", 1 } };

        public async Task Run(ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\The Store");

            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                driver.Navigate().GoToUrl((string)links[0, 0]);
                int counter = 1;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                while (true)
                {

                    try
                    {
                        By selector = By.CssSelector("div[class='product-tile restored']");
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
                                string name = $"{RemoveSpecialCharacters(eName.Text)} ({conditionName.Text})";
                                string link = eLink.GetAttribute("href");
                                string image = "https://thestore.com" + eImage.GetAttribute("data-src").Replace("height=300", "height=400");
                                decimal price = decimal.Parse(ePriceWhole.Text.Replace("$", ""));
                                int condition = 1;
                                bool save = true;
                                int shop = 3;
                                int type = (int)links[0, 1];


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
                                await WriteLogs($"\nTheStore: ---> {e}");
                            }

                        });
                    }
                    catch (WebDriverTimeoutException)
                    {
                        await ScreensShot(driver, "The Store", 0, counter);
                    }
                    catch (StaleElementReferenceException e)
                    {
                        await WriteLogs($"\nTheStore: ---> {e}");
                    }


                    Console.WriteLine($"\nThe Store\t{counter}");
                    counter++;
                    try
                    { 
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[class='round-button round-button__next']"))).Click();
                    }

                    catch (WebDriverTimeoutException)
                    {
                        await ScreensShot(driver, "TheStore", 0, counter, "getElementsByClassName('pagination is-centered')[0]");
                        driver.Quit();
                        break;
                    }

                }

            }
        }

    }
}
