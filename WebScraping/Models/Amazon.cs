using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebScraping.Extensions;
using WebScraping.Services;
using WebScraping.Utils;
using Item = WebScraping.Entities.Item;
using Shop = WebScraping.Emuns.Shop;

namespace WebScraping.Models
{
    public class Amazon : ItemService
    {
        private static ILogger _logger;
        private static string error;
        private static object[,] links =
        {
            {"https://www.amazon.com/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_n_condition-type%3A6503240011%2Cp_36%3A-20000%2Cp_n_feature_twelve_browse-bin%3A14674909011%7C14674910011%7C14674911011%7C17352550011%2Cp_n_feature_nineteen_browse-bin%3A9521921011%2Cp_72%3A2491149011&dc&qid=1648600689&rnid=2491147011", Emuns.Type.Phone},
            {"https://www.amazon.com/s?keywords=Micro+SD+Memory+Cards&i=computers&rh=n%3A3015433011%2Cp_72%3A1248879011%2Cp_89%3ASAMSUNG%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648600035&rnid=2224369011&ts_id=3015433011",Emuns.Type.Memory},
            {"https://www.amazon.com/s?i=electronics&bbn=13447451&rh=n%3A13447451%2Cp_89%3AAmazon%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-6000&dc&language=es&qid=1652512888&rnid=386442011",Emuns.Type.Streaming},

            /*{"https://www.amazon.com/s?k=Recording+Microphones+%26+Accessories&i=mi&rh=p_n_deal_type%3A23566064011%2Cp_n_specials_match%3A21213697011&dc&qid=1654485545&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.Microphone},
            {"https://www.amazon.com/s?k=Tripod+Accessories&i=photo&rh=n%3A502394%2Cp_n_deal_type%3A23566064011%2Cp_36%3A900-5000%2Cp_n_condition-type%3A2224371011%2Cp_n_specials_match%3A21213697011&dc&qid=1654485609&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",Emuns.Type.Accessory},
            {"https://www.amazon.com/s?k=Wireless+%26+Streaming+Audio+Systems&i=electronics&rh=n%3A172282%2Cp_72%3A1248879011%2Cp_36%3A-10000%2Cp_n_condition-type%3A2224371011%2Cp_n_deal_type%3A23566064011%2Cp_n_specials_match%3A21213697011&dc&qid=1654485652&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Home+Audio+Sound+Bars&i=electronics&rh=p_36%3A-9900%2Cp_n_deal_type%3A23566064011%2Cp_n_specials_match%3A21213697011&dc&qid=1654485676&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Outdoor+Speakers&i=electronics&rh=n%3A12097477011%2Cp_n_deal_type%3A23566065011%2Cp_36%3A-20000%2Cp_n_specials_match%3A21213697011&dc&qid=1654485698&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Home+Audio+Subwoofers&i=electronics&rh=n%3A172568%2Cp_n_deal_type%3A23566065011%2Cp_36%3A-20000%2Cp_n_specials_match%3A21213697011&dc&qid=1654485721&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Center-Channel+Speakers&i=electronics&rh=p_n_deal_type%3A23566065011%2Cp_36%3A-20000%2Cp_n_specials_match%3A21213697011&dc&qid=1654485746&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Bookshelf+Speakers&i=electronics&rh=p_n_deal_type%3A23566065011%2Cp_36%3A-20000%2Cp_n_specials_match%3A21213697011&dc&qid=1654485769&rnid=21213696011&ref=sr_nr_p_n_speciach_1", Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Recording+Headphone+%26+In-Ear+Audio+Monitors&i=mi&rh=n%3A11974971%2Cp_n_deal_type%3A23566065011%2Cp_36%3A-10000&dc&qid=1652511869&rnid=386685011&ref=sr_nr_p_36_5",Emuns.Type.Headphone},ls_match_1", Emuns.Type.Speaker},
            {"https://www.amazon.com/s?k=Portable+Bluetooth+Speakers&i=electronics&bbn=172282&rh=p_n_deal_type%3A23566065011%2Cp_36%3A-10000%2Cp_n_specials_match%3A21213697011&dc&qid=1654485791&rnid=21213696011&ref=sr_nr_p_n_specials_mat
            {"https://www.amazon.com/s?keywords=Earbud+%26+In-Ear+Headphones&i=electronics&rh=n%3A12097478011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-10000%2Cp_89%3AJBL%7CMonster%7CPanasonic%7CSAMSUNG%7CSony%2Cp_n_deal_type%3A23566065011%2Cp_n_specials_match%3A21213697011&dc&c=ts&qid=1654485861&rnid=21213696011&ts_id=12097478011&ref=sr_nr_p_n_specials_match_1", (int) Emuns.Type.Headphone},
            {"https://www.amazon.com/s?k=On-Ear+Headphones&i=electronics&rh=p_n_deal_type%3A23566064011%2Cp_n_specials_match%3A21213697011&dc&qid=1654485888&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.Headphone},
            {"https://www.amazon.com/s?k=Over-Ear+Headphones&i=electronics&rh=p_n_deal_type%3A23566064011%2Cp_n_specials_match%3A21213697011&dc&qid=1654485915&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.Headphone},
            {"https://www.amazon.com/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_n_deal_type%3A23566064011%2Cp_36%3A-20000%2Cp_n_specials_match%3A21213697011&dc&language=en_US&qid=1654485998&rnid=21213696011&ref=sr_nr_p_n_specials_match_1", Emuns.Type.TV},
            {"https://www.amazon.com/-/es/s?k=memory+usb+kingston&dc&__mk_es_US=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1652323122&ref=sr_ex_p_n_deal_type_0",  Emuns.Type.Memory},
            {"https://www.amazon.com/s?k=memory+usb+samsung&i=computers&bbn=3151491&rh=n%3A3151491&dc&language=es&__mk_es_US=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1652323252&rnid=2528832011&ref=sr_nr_p_89_1",  Emuns.Type.Memory}*/
        };


        public static void Run()
        {
            _logger = LogConsole.CreateLogger<Amazon>();
            string option = $@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Amazon";
            using (IWebDriver driver = Selenium.CreateChromeDriver(option))
            {
                for (int i = 0; i < links.Length / 2; i++)
                {
                    int counter = 1;
                    driver.Navigate().GoToUrl((string)links[i, 0]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                    HashSet<Item> itemList = new HashSet<Item>();

                    while (true)
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

                                    string link = Uri.UnescapeDataString(eLink.GetAttribute("href"));
                                    if (link.Contains("/ref"))
                                        link = link.Substring(0, link.IndexOf("/ref"));

                                    var item = new Item();
                                    item.Name = eName.Text.RemoveSpecialCharacters();
                                    item.Link = link;
                                    item.Image = eImage.GetAttribute("src").Replace("218", "320");
                                    item.Price = decimal.Parse(ePriceWhole.Text.Replace(",", "") + "." + ePriceFraction.Text);
                                    item.Shop = (int)Shop.Amazon;
                                    item.Type = (int)links[i, 1];

                                    if (await item.CanBeSave())
                                    {
                                        itemList.Add(item);
                                    }
                                    

                                }
                                catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                                {
                                    if (!e.Message.Contains("a-price-whole"))
                                    {
                                        error = $"URL: {i} | {e.Message}";
                                        _logger.LogWarning(error);
                                        LogFile.Write<Amazon>(error);
                                    }
                                }
                            });
                        }
                        catch (WebDriverTimeoutException e)
                        {
                            string message = "Error";
                            error = $"URL: {i} | {e.Message}";
                            ScreenShot.Take(driver, ref message, ref i, ref counter);
                            _logger.LogWarning(error);
                            LogFile.Write<Amazon>(error);
                        }

                        try
                        {
                            wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='s-pagination-item s-pagination-next s-pagination-button s-pagination-separator'"))).Click();

                        }
                        catch (WebDriverTimeoutException)
                        {

                            var by = "getElementsByClassName('s-pagination-item s-pagination-next s-pagination-button s-pagination-separator')[0]";
                            string shop = "Amazon";
                            ScreenShot.TakeAtBottom(driver, ref shop, ref i, ref counter, ref by);
                            break;
                        }

                        _logger.LogInformation($"{i}\t| {counter}\t| {itemList.Count}");
                        counter++;

                    }

                    Save(ref itemList);
                }
                driver.Quit();

            }
        }
    }
}
