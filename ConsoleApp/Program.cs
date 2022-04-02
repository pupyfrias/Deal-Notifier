using DataBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleApp
{
    class Program
    {
        public static ArrayList checkList = new ArrayList();
        public static List<string>  blackList = new List<string>();
        public static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        public static string[] filterList = {"tracfone", "total wireless", "net10", "simple mobile", "straight talk"};
        public static string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };
        public static object[,] amazonLinkList = {{"https://www.amazon.com/s?k=Recording+Microphones+%26+Accessories&i=mi&rh=n%3A11974521%2Cp_n_specials_match%3A21213697011%2Cp_72%3A1248939011%2Cp_n_condition-type%3A404228011&dc=&c=ts&qid=1648604195&rnid=386685011&ts_id=11974521&ref=sr_nr_p_36_5&low-price=&high-price=100",6},
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
                                                  {"https://www.amazon.com/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_n_condition-type%3A2224371011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011&dc&qid=1648597881&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",2}};
                                                     

        static object[,] ebayLinkList = {{"https://www.ebay.com/sch/i.html?_dcat=9355&_fsrp=1&_blrs=recall_filtering&_from=R40&LH_TitleDesc=0&LH_ItemCondition=1000&Brand=Samsung%7CApple%7CMotorola%7CAlcatel%7CXiaomi%7CHuawei%7CLG&_nkw=phones&_sacat=9355&Network=AT%2526T%7CCricket%2520Wireless%7CSprint%7CT%252DMobile%7CUnlocked%7CBoom%2520Mobile%7CBoost%2520Mobile%7CMetro%7CVerizon%7CUS%2520Mobile%7CU%252ES%252E%2520Cellular%7CVirgin%2520Mobile&LH_BIN=1&Storage%2520Capacity=32%2520GB%7C64%2520GB%7C256%2520GB%7C512%2520GB%7C128%2520GB&_sop=15&_udhi=120&rt=nc&_stpos=19720&_sadis=2000&LH_PrefLoc=99&_fspt=1&_ipg=240",1 },
                                        { "https://www.ebay.com/sch/i.html?_dcat=11071&_fsrp=1&rt=nc&_from=R40&LH_PrefLoc=99&LH_ItemCondition=1000&_stpos=19720&_nkw=tv&_sacat=32852&LH_BIN=1&_fspt=1&Screen%2520Size=60%252D69%2520in%7C20%252D29%2520in%7C40%252D49%2520in%7C30%252D39%2520in%7C50%252D59%2520in%7C70%252D80%2520in&_udhi=200&_sadis=2000&_ipg=240" ,2}
                                        };

        static object [,] theStoreLinkList = { { "https://thestore.com/c/refurbished-cell-phones-58", 1 } };
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"/logs";

    static async Task Main(string[] args)
    {

        var listItems = new List<SP_GetAllLinks_Result>();

        using (WebScrapingEntities context = new WebScrapingEntities())
        {
            context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            blackList = context.SP_GET_BLACK_LIST().ToList();
        }

        try
        {
            var task1 = Task.Run(async () =>
            {   
                await Selenium("amazon", amazonLinkList);
            });


            var task2 = Task.Run(async () =>
            {
                await Selenium("thestore", theStoreLinkList);
            });

            var task3 = Task.Run(async () =>
            {
                await Selenium("eBay",ebayLinkList);
                
            });

            await Task.WhenAll(task1, task2, task3);

            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                listItems =  context.SP_GetAllLinks().ToList();
            }

        

            Parallel.ForEach(listItems, (i) =>
            {
                using (WebScrapingEntities context = new WebScrapingEntities())
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                    if (checkList.Contains(i.link))
                    {
                        context.SP_UPDATE_STATUS(i.id, 1);
                    }
                    else
                    {
                        context.SP_UPDATE_STATUS(i.id, 2);
                    }
                }
            });
             
            Console.WriteLine("DONE");   
            SystemSounds.Asterisk.Play();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);   
        }
                  
    }


        private async static Task Selenium(string web, object[,] links)
        {
            await Task.Run(() => {
                ChromeOptions options = new ChromeOptions();
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
                options.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                      "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                      "--window-size=1920,1080", "--disable-browser-side-navigation", "--headless", "--log-level=3", "--silent");
                options.AddExcludedArgument("enable-automation");
                
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;

                if (web == "amazon")
                {
                    new Amazon(links, options, service);
                }
                else if (web =="thestore")
                {
                    new TheStore(links, options, service);
                }
                else if (web == "eBay")
                {
                    new Ebay(links, options, service);
                }
            });
            
        }
         
        public static async Task SaveOrUpdate(bool save, string name, string link, string image, decimal price,
            int condition,string shop, int type)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                //var data = await Task.Run(() => context.SP_GET_ONE(link).AsEnumerable().FirstOrDefault());
                var data = await context.Items.SingleOrDefaultAsync(i => i.LINK == link);
                checkList.Add(link);

                if (data != null)
                {
                    decimal oldPrice = (decimal) ((decimal) data.OLD_PRICE > 0? data.OLD_PRICE: data.PRICE);
                    decimal saving =0, saving_percent= 0;
                    bool validate = false;

                    if (oldPrice > price)
                    {
                       saving = oldPrice - price;
                       saving_percent = saving / oldPrice * 100;
                       validate = true;
                    }
                    else if (oldPrice < price)
                    {
                        oldPrice = 0;
                        validate = true;
                    }
                    else if (name!= data.NAME)
                    {
                        validate = true;
                    }
                    

                    if (true)//validate)
                    {
                        context.SP_UPDATE_PRICE(data.ID, price, oldPrice, saving, saving_percent,name, type);//quitar type
                        await context.SaveChangesAsync();
                    }
                    
                    
                }
                else if (save)
                {
                    context.SP_ADD(name, price, link, condition, shop, image, type);
                    await context.SaveChangesAsync();
                }
                
            }
        }
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[,.+'\":;]", "", RegexOptions.Compiled);
        }

        public void ScreensShot(IWebDriver driver, string shop, int counter)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"{path}/{shop}_{counter}.png", ScreenshotImageFormat.Png);
        }

        public async void WriteLogs(string log, string shop)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "SHARED_BY_ALL_PROCESSES");
            string logPath = $"{path}/log {shop} {DateTime.Now.ToShortDateString().Replace("/", "_")}.txt";
            
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path);}
            if (!File.Exists(logPath)) { using (FileStream fs = new FileStream(logPath, FileMode.Create));}
           
            waitHandle.WaitOne();
            var sw = new StreamWriter(logPath, true);
            await sw.WriteLineAsync(log);
            sw.Close();
            waitHandle.Set();
        }

    }
}
