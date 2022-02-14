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
    internal class TheStore: Program
    {
        public TheStore(string url, ChromeOptions options)
        {
            options.AddArgument($"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}/User Data/The Store");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);
                bool removeElement = true;
                int counter = 1;

                while (true)
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
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
                            string image = "https://thestore.com" + eImage.GetAttribute("data-src");
                            decimal price = decimal.Parse(ePriceWhole.Text.Replace("$", ""));
                            int condition = 1;
                            bool save = true;
                            string shop = "TheStore";


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
                        catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                        {
                            Console.Write($"ERROR THE STORE: ---> {e.Message}\n");
                        }

                    });

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

                    catch (WebDriverTimeoutException e)
                    {
                        break;
                    }

                }

            }

        }
    }
}
