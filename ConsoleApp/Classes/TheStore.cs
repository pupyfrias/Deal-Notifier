using ConsoleApp.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class TheStore: Method, IRun
    {
    
        public  async Task Run(object[,] links, ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\The Store");

            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                driver.Navigate().GoToUrl((string)links[0, 0]);
                bool removeElement = true;
                int counter = 1;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                while (true)
                {

                    try
                    {
                        By selector = By.CssSelector("div[class='product-tile restored']");
                        wait.Until(ExpectedConditions.ElementToBeClickable(selector));
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
                                string shop = "TheStore";
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
                                await WriteLogs($"ERROR: ---> {e.Message}", "The Store ");
                            }

                        });
                    }
                    catch (WebDriverTimeoutException)
                    {
                        ScreensShot(driver, "The Store", counter);
                    }
                    catch (StaleElementReferenceException e)
                    {
                        await WriteLogs($"ERROR: ---> {e.Message}", "The Store");
                    }


                    Console.WriteLine($"\nThe Store {counter}");
                    counter++;
                    try
                    {
                        if (removeElement)
                        {
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            try { js.ExecuteScript("document.getElementsByClassName('acsb-trigger acsb-bg-lead acsb-trigger-size-medium acsb-trigger-position-x-right acsb-trigger-position-y-bottom acsb-ready').forEach(e=> e.remove())"); } catch (Exception) { }
                            try { js.ExecuteScript("document.getElementById('cc-button').remove()"); } catch (Exception) { }
                            try { js.ExecuteScript("document.getElementsByClassName('hello-bar').forEach(e=> e.remove())"); } catch (Exception) { }
                            removeElement = false;
                        }
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[class='round-button round-button__next']"))).Click();
                    }

                    catch (WebDriverTimeoutException)
                    {
                        ScreensShot(driver, "TheStore", counter);
                        driver.Quit();
                        break;
                    }

                }

            }
        }

    }
}
