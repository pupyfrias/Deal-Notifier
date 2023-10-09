using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Heplers;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Serilog;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Condition = DealNotifier.Core.Application.Enums.Condition;
using OnlineStore = DealNotifier.Core.Application.Enums.OnlineStore;
using Status = DealNotifier.Core.Application.Enums.Status;
using ItemType = DealNotifier.Core.Application.Enums.ItemType;
using DealNotifier.Core.Application.Interfaces.Services;

namespace DealNotifier.Infrastructure.Persistence.Models
{
    public class TheStore
    {
        private static ILogger _logger;
        private IItemSyncService _itemSyncService;
        private static object[,] links = { { "https://thestore.com/c/refurbished-cell-phones-58?condition=Brand%20New&showMore=0", ItemType.Phone } };

        public TheStore(ILogger logger, IItemSyncService itemSyncService)
        {
            _itemSyncService = itemSyncService;
        }

        public void Run()
        {
            _logger = Logger.CreateLogger().ForContext<TheStore>();
            string option = $@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\The Store";

            using (IWebDriver driver = SeleniumTools.CreateChromeDriver(option))
            {
                driver.Navigate().GoToUrl((string)links[0, 0]);
                int counter = 1;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                ConcurrentBag<ItemCreateRequest> itemList = new ConcurrentBag<ItemCreateRequest>();

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

                                ItemCreateRequest item = new ItemCreateRequest();
                                item.Name = $"{eName.Text.RemoveSpecialCharacters()} {conditionName.Text.RemoveSpecialCharacters()}";
                                item.Link = Helper.GetLocalPath(eLink.GetAttribute("href"));
                                item.Image = "https://thestore.com" + eImage.GetAttribute("data-src").Replace("height=300", "height=400");
                                item.Price = decimal.Parse(ePriceWhole.Text.Replace("$", ""));
                                item.OnlineStoreId = (int)OnlineStore.TheStore;
                                item.ItemTypeId = (int)links[0, 1];
                                item.ConditionId = (int)Condition.New;
                                item.StockStatusId = (int)Status.InStock;

                                if (await item.CanBeSaved())
                                {
                                    item.SetCondition();
                                    itemList.Add(item);
                                }
                            }
                            catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                            {
                                _logger.Warning(e.ToString());
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        string shop = "The Store";
                        int link = 0;

                        _logger.Error(e.ToString());
                        driver.TakeScreenShot(ref shop, ref link, ref counter);
                    }

                    _logger.Information($"0\t| {counter}\t| {itemList.Count}");
                    counter++;
                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[class='round-button round-button__next']"))).Click();
                    }
                    catch (WebDriverTimeoutException)
                    {
                        string shop = "The Store";
                        string by = "getElementsByClassName('pagination is-centered')[0]";
                        int link = 0;

                        driver.TakeScreemShotAtBottom(ref shop, ref link, ref counter, ref by);
                        driver.Quit();
                        break;
                    }
                }

               _itemSyncService.SaveOrUpdate(itemList);
            }
        }
    }
}