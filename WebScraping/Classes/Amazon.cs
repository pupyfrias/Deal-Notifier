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
            {"https://www.amazon.com/s?k=Recording+Microphones+%26+Accessories&i=mi&rh=n%3A11974521%2Cp_n_specials_match%3A21213697011%2Cp_72%3A1248939011%2Cp_n_condition-type%3A404228011&dc=&c=ts&qid=1648604195&rnid=386685011&ts_id=11974521&ref=sr_nr_p_36_5&low-price=&high-price=100",6},
            {"https://www.amazon.com/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_n_condition-type%3A6503240011%2Cp_36%3A-20000%2Cp_n_feature_twelve_browse-bin%3A14674909011%7C14674910011%7C14674911011%7C17352550011%2Cp_n_feature_nineteen_browse-bin%3A9521921011%2Cp_72%3A2491149011&dc&qid=1648600689&rnid=2491147011&ref=sr_nr_p_72_1",1},
            {"https://www.amazon.com/s?keywords=Tripod+Accessories&i=photo&rh=n%3A3347771%2Cp_n_specials_match%3A21213697011%2Cp_72%3A1248879011&dc&c=ts&qid=1648600102&rnid=1248877011&ts_id=3347771&ref=sr_nr_p_72_1",8},
            {"https://www.amazon.com/s?keywords=Micro+SD+Memory+Cards&i=computers&rh=n%3A3015433011%2Cp_72%3A1248879011%2Cp_89%3ASAMSUNG%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648600035&rnid=2224369011&ts_id=3015433011&ref=sr_nr_p_n_condition-type_1",3},
            {"https://www.amazon.com/s?keywords=Wireless+%26+Streaming+Audio+Systems&i=electronics&rh=n%3A322215011%2Cp_n_specials_match%3A21213697011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648599708&rnid=2224369011&ts_id=322215011&ref=sr_nr_p_n_condition-type_1",5},
            {"https://www.amazon.com/s?k=Home+Audio+Sound+Bars&i=electronics&rh=n%3A3237803011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011&dc=&c=ts&qid=1648599657&rnid=386442011&ts_id=3237803011&ref=sr_nr_p_36_2&low-price=&high-price=100",5},
            {"https://www.amazon.com/s?k=Outdoor+Speakers&i=electronics&rh=n%3A12097477011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011&dc=&c=ts&qid=1648599583&rnid=386442011&ts_id=12097477011&ref=sr_nr_p_36_5&low-price=&high-price=100",5},
            {"https://www.amazon.com/s?k=Home+Audio+Subwoofers&i=electronics&rh=n%3A172568%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011&dc=&c=ts&qid=1648599442&rnid=386442011&ts_id=172568&ref=sr_nr_p_36_5&low-price=&high-price=150",5},
            {"https://www.amazon.com/s?keywords=Center-Channel+Speakers&i=electronics&rh=n%3A3236452011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011&dc&c=ts&qid=1648599328&rnid=21213696011&ts_id=3236452011&ref=sr_nr_p_n_specials_match_1",5},
            {"https://www.amazon.com/s?keywords=Bookshelf+Speakers&i=electronics&rh=n%3A3236451011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_n_specials_match%3A21213697011&dc&c=ts&qid=1648599241&rnid=21213696011&ts_id=3236451011&ref=sr_nr_p_n_specials_match_1",5},
            {"https://www.amazon.com/s?keywords=Portable+Bluetooth+Speakers&i=electronics&rh=n%3A7073956011%2Cp_n_specials_match%3A21213697011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648599185&rnid=2224369011&ts_id=7073956011&ref=sr_nr_p_n_condition-type_1",5},
            {"https://www.amazon.com/s?k=Recording+Headphone+%26+In-Ear+Audio+Monitors&i=mi&rh=n%3A11974971%2Cp_72%3A1248939011%2Cp_n_condition-type%3A404228011&dc=&c=ts&qid=1648599024&rnid=386685011&ts_id=11974971&ref=sr_nr_p_36_5&low-price=&high-price=100",4},
            {"https://www.amazon.com/s?i=electronics&bbn=509318&rh=n%3A509318%2Cp_n_specials_match%3A21213697011%2Cp_72%3A2661618011%2Cp_n_condition-type%3A6461716011&dc&qid=1648598900&rnid=6461714011&ref=sr_nr_p_n_condition-type_1",4},
            {"https://www.amazon.com/s?keywords=Earbud+%26+In-Ear+Headphones&i=electronics&rh=n%3A12097478011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-10000%2Cp_89%3AJBL%7CMonster%7CPanasonic%7CSAMSUNG%7CSony&dc&c=ts&qid=1648598737&rnid=2528832011&ts_id=12097478011&ref=sr_nr_p_89_4",4},
            {"https://www.amazon.com/s?keywords=On-Ear+Headphones&i=electronics&rh=n%3A12097480011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648598442&rnid=2224369011&ts_id=12097480011&ref=sr_nr_p_n_condition-type_1",4},
            {"https://www.amazon.com/s?keywords=Over-Ear+Headphones&i=electronics&rh=n%3A12097479011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011%2Cp_n_condition-type%3A2224371011&dc&c=ts&qid=1648598374&rnid=2224369011&ts_id=12097479011&ref=sr_nr_p_n_condition-type_1",4},
            {"https://www.amazon.com/s?i=electronics&bbn=12097486011&rh=n%3A172282%2Cn%3A13900851%2Cn%3A513014%2Cn%3A12097486011%2Cn%3A667846011%2Cn%3A172563%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011&lo=image&dc&pf_rd_i=12097486011&pf_rd_m=ATVPDKIKX0DER&pf_rd_p=193b68f4-40b1-46b8-b92d-cbf6ce8d1697&pf_rd_r=66TR224D6DY4PXB64A03&pf_rd_s=merchandised-search-5&pf_rd_t=101&qid=1648598224&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",5},
            {"https://www.amazon.com/s?keywords=Home+Theater+Systems&i=electronics&rh=n%3A281056%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_n_specials_match%3A21213697011&dc&c=ts&qid=1648598000&rnid=21213696011&ts_id=281056&ref=sr_nr_p_n_specials_match_1",5},
            {"https://www.amazon.com/s?i=electronics&bbn=13447451&rh=n%3A13447451%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_n_specials_match%3A21213697011&dc&qid=1648597962&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",7},
            {"https://www.amazon.com/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_n_condition-type%3A2224371011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011&dc&qid=1648597881&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",2},
            {"https://www.amazon.com/-/es/s?k=memory+usb&i=electronics&rh=n%3A3151491%2Cp_89%3AKingston%7CSAMSUNG%7CSanDisk%2Cp_72%3A1248879011%2Cp_n_size_browse-bin%3A10285016011%7C10285018011%7C10285020011%7C1259716011%2Cp_n_condition-type%3A2224371011&dc&language=es&__mk_es_US=%C3%85M%C3%85%C5%BD%C3%95%C3%91&crid=3KX7CUPAFF5EL&qid=1650053451&rnid=2224369011&sprefix=memory+usb%2Celectronics%2C187&ref=sr_nr_p_n_condition-type_1",3}
        };

        public async Task Run(ChromeOptions options, ChromeDriverService service)
        {
            options.AddArguments($@"--user-data-dir={AppDomain.CurrentDomain.BaseDirectory}User Data\Amazon");
            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                for (int i = 0; i < links.Length / 2; i++)
                {
                    int counter = 1;
                    bool run = true;

                    /*    try
                        {
                            driver.Navigate().GoToUrl((string)links[i, 0]);
                        }
                        catch (WebDriverException e)
                        {
                            await WriteLogs($"BAD URL: ---> {e.Message.Trim()} | url:{(string)links[i, 0]}");
                            run = false;
                            driver.Quit();
                        }*/


                    driver.Navigate().GoToUrl((string)links[i, 0]);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
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
                                    string name = RemoveSpecialCharacters(eName.Text);
                                    string link = eLink.GetAttribute("href");
                                    link = link.Substring(0, link.IndexOf("/ref"));
                                    string image = eImage.GetAttribute("src").Replace("218", "320");
                                    decimal price = decimal.Parse(ePriceWhole.Text.Replace(",", "") + "." + ePriceFraction.Text);
                                    int condition = 1;
                                    bool save = true;
                                    int shop = 1;
                                    int type = (int)links[i, 1];


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
                                    if (!e.Message.Contains("a-price-whole"))
                                    {
                                        await WriteLogs($"Amazon: --->  | URL: {i} | {e.Message.Trim()}");
                                    }

                                }
                                /*catch (EntityCommandExecutionException)
                                {
                                    Console.WriteLine(error);
                                }*/

                            });
                        }
                        catch (WebDriverTimeoutException e)
                        {
                            await ScreensShot(driver, "Error", i, counter);
                            await WriteLogs($"Amazon: --->  | URL: {i} | {e.Message.Trim()}");
                        }


                        try
                        {
                            wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[class='s-pagination-item s-pagination-next s-pagination-button s-pagination-separator'"))).Click();

                        }
                        catch (WebDriverTimeoutException)
                        {
                            await ScreensShot(driver, "Amazon", i, counter);
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
