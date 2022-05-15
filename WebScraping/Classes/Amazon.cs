using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebScraping.Classes;



namespace WebScraping
{
    internal class Amazon : Method, IRun
    {
        public object[,] links =
        {
            {"https://www.amazon.com/s?k=Recording+Microphones+%26+Accessories&i=mi&rh=p_n_deal_type%3A23566064011&dc&qid=1652510651&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",6},
            {"https://www.amazon.com/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_n_condition-type%3A6503240011%2Cp_36%3A-20000%2Cp_n_feature_twelve_browse-bin%3A14674909011%7C14674910011%7C14674911011%7C17352550011%2Cp_n_feature_nineteen_browse-bin%3A9521921011%2Cp_72%3A2491149011&dc&qid=1648600689&rnid=2491147011&ref=sr_nr_p_72_1",1},
            {"https://www.amazon.com/s?k=Tripod+Accessories&i=photo&rh=n%3A502394%2Cp_n_deal_type%3A23566064011%2Cp_36%3A900-5000%2Cp_n_condition-type%3A2224371011&dc&qid=1652511037&rnid=2224369011&ref=sr_nr_p_n_condition-type_1",8},
            {"https://www.amazon.com/s?keywords=Micro+SD+Memory+Cards&i=computers&rh=n%3A3015433011%2Cp_72%3A1248879011%2Cp_89%3ASAMSUNG%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648600035&rnid=2224369011&ts_id=3015433011&ref=sr_nr_p_n_condition-type_1",3},
            {"https://www.amazon.com/s?k=Wireless+%26+Streaming+Audio+Systems&i=electronics&rh=n%3A172282%2Cp_72%3A1248879011%2Cp_36%3A-10000%2Cp_n_condition-type%3A2224371011%2Cp_n_deal_type%3A23566064011&dc&qid=1652511332&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",5},
            {"https://www.amazon.com/s?k=Home+Audio+Sound+Bars&i=electronics&rh=p_36%3A-9900%2Cp_n_deal_type%3A23566064011&dc&qid=1652511434&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",5},
            {"https://www.amazon.com/s?k=Outdoor+Speakers&i=electronics&rh=n%3A12097477011%2Cp_n_deal_type%3A23566065011%2Cp_36%3A-20000&dc&qid=1652511530&rnid=386442011&ref=sr_nr_p_36_5",5},
            {"https://www.amazon.com/s?k=Home+Audio+Subwoofers&i=electronics&rh=n%3A172568%2Cp_n_deal_type%3A23566065011%2Cp_36%3A-20000&dc&qid=1652511606&rnid=386442011&ref=sr_nr_p_36_5",5},
            {"https://www.amazon.com/s?k=Center-Channel+Speakers&i=electronics&rh=p_n_deal_type%3A23566065011%2Cp_36%3A-20000&dc&qid=1652511657&rnid=386442011&ref=sr_nr_p_36_5",5},
            {"https://www.amazon.com/s?k=Bookshelf+Speakers&i=electronics&rh=p_n_deal_type%3A23566065011%2Cp_36%3A-20000&dc&qid=1652511722&rnid=386442011&ref=sr_nr_p_36_5",5},
            {"https://www.amazon.com/s?k=Portable+Bluetooth+Speakers&i=electronics&bbn=172282&rh=p_n_deal_type%3A23566065011%2Cp_36%3A-10000&dc&qid=1652511802&ref=sr_ex_p_89_0",5},
            {"https://www.amazon.com/s?k=Recording+Headphone+%26+In-Ear+Audio+Monitors&i=mi&rh=n%3A11974971%2Cp_n_deal_type%3A23566065011%2Cp_36%3A-10000&dc&qid=1652511869&rnid=386685011&ref=sr_nr_p_36_5",4},
            {"https://www.amazon.com/s?keywords=Earbud+%26+In-Ear+Headphones&i=electronics&rh=n%3A12097478011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-10000%2Cp_89%3AJBL%7CMonster%7CPanasonic%7CSAMSUNG%7CSony%2Cp_n_deal_type%3A23566065011&dc&c=ts&qid=1652511914&rnid=23566063011&ts_id=12097478011&ref=sr_nr_p_n_deal_type_1",4},
            {"https://www.amazon.com/s?k=On-Ear+Headphones&i=electronics&rh=p_n_deal_type%3A23566064011&dc&qid=1652322682&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",4},
            {"https://www.amazon.com/s?k=Over-Ear+Headphones&i=electronics&rh=p_n_deal_type%3A23566064011&dc&qid=1652322739&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",4},
            {"https://www.amazon.com/s?i=electronics&bbn=13447451&rh=n%3A13447451%2Cp_89%3AAmazon%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-6000&dc&language=es&qid=1652512888&rnid=386442011&ref=sr_nr_p_36_5",7},
            {"https://www.amazon.com/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_n_deal_type%3A23566064011%2Cp_36%3A-20000&dc&qid=1652512583&rnid=8589203011&ref=sr_nr_p_36_2",2},
            {"https://www.amazon.com/-/es/s?k=memory+usb+kingston&dc&__mk_es_US=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1652323122&ref=sr_ex_p_n_deal_type_0",3},
            {"https://www.amazon.com/s?k=memory+usb+samsung&i=computers&bbn=3151491&rh=n%3A3151491&dc&language=es&__mk_es_US=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1652323252&rnid=2528832011&ref=sr_nr_p_89_1",3},
        };

        public void Run(ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Amazon");
            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                for (int i = 0; i < links.Length / 2; i++)
                {
                    int counter = 1;
                    bool run = true;

                    driver.Navigate().GoToUrl((string)links[i, 0]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

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
                                    string name = eName.Text;
                                    RemoveSpecialCharacters(ref name);
                                    string link = eLink.GetAttribute("href");
                                    link = link.Substring(0, link.IndexOf("/ref"));
                                    string image = eImage.GetAttribute("src").Replace("218", "320");
                                    decimal price = decimal.Parse(ePriceWhole.Text.Replace(",", "") + "." + ePriceFraction.Text);
                                    int condition = 1;
                                    bool save = true;
                                    int shop = 1;
                                    int type = (int)links[i, 1];


                                    if (blackList.Contains(link))
                                    {
                                        save = false;
                                    }
                                    else
                                    {
                                        var task1 = Task.Run(() =>
                                        {
                                            Parallel.ForEach(conditionList, (data, state) =>
                                            {
                                                if (name.ToLower().Contains(data))
                                                {
                                                    condition = 2;
                                                    state.Break();
                                                }
                                            });
                                        });

                                        var task2 = Task.Run(() =>
                                        {
                                            Parallel.ForEach(filterList, (data, state) =>
                                            {
                                                if (name.ToLower().Contains(data))
                                                {
                                                    save = false;
                                                    state.Break();
                                                }
                                            });
                                        });

                                        await Task.WhenAll(task1, task2);
                                    }

                                    SaveOrUpdate(ref save, ref name, link, ref image, ref  price, ref condition,ref shop, ref type);
                                }
                                catch (Exception e) when (e is NoSuchElementException | e is StaleElementReferenceException)
                                {
                                    if (!e.Message.Contains("a-price-whole"))
                                    {
                                       WriteLogs($"Amazon: --->  | URL: {i} | {e}");
                                    }

                                }
                            });
                        }
                        catch (WebDriverTimeoutException e)
                        {
                            string message = "Error";
                            ScreensShot(driver, ref message, ref i, ref counter);
                            WriteLogs($"Amazon: --->  | URL: {i} | {e}");
                        }

                        try
                        {
                            wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='s-pagination-item s-pagination-next s-pagination-button s-pagination-separator'"))).Click();

                        }
                        catch (WebDriverTimeoutException)
                        {
                            
                            var by = "getElementsByClassName('s-pagination-item s-pagination-next s-pagination-button s-pagination-separator')[0]";
                            string shop = "Amazon";
                            ScreensShot(driver,ref shop ,ref i,ref counter, ref by);
                            break;
                        }

                        Console.WriteLine($"\nAmazon {i}\t{counter} ");
                        counter++;

                    }

                }
                driver.Quit();

            }
        }
    }
}
