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
    internal class Ebay: Program
    {
        public Ebay(string url, ChromeOptions options)
        {
            options.AddArgument($"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}/User Data/Ebay");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);
                int counter = 1;

                while (true)
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    By selector = By.CssSelector("li[class='s-item s-item--large s-item--bgcolored']");
                    wait.Until(ExpectedConditions.ElementToBeClickable(selector));
                    Thread.Sleep(2000);

                    ReadOnlyCollection<IWebElement> elements = driver.FindElements(selector);
                    Parallel.ForEach(elements, async (element) =>
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
                            Console.Write($"ERROR EBAY: ---> {e.Message}\n");
                        }

                    });

                    Console.WriteLine($"\nEbay {counter}");
                    counter++;
                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='pagination__next icon-link']"))).Click();
                        if (counter > 55) { break; }
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
