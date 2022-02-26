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
        public static int amazonLinkPositon = 1 , ebayLinkPositon = 1;
        public static ArrayList checkList = new ArrayList();
        public static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        public static string[] filterList = {"tracfone", "total wireless", "net10", "simple mobile", "straight talk"};
        public static string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };
        public static string[] amazonLinkList = {"https://www.amazon.com/-/es/s?i=computers&bbn=17923671011&rh=n%3A17923671011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A-20000%2Cp_n_condition-type%3A2224371011%2Cp_89%3AARCTIC%7CCorsair%7CCrucial%7CElgato%7CNoctua%7CSAMSUNG%7CSamsung+Electronics%7CSeagate%2Cp_n_deal_type%3A23566065011&dc&language=es&brr=1&pd_rd_r=d14c0261-e650-46e8-a548-15bac2fa9b3d&pd_rd_w=8M1il&pd_rd_wg=cdnkB&qid=1645624352&rd=1&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?keywords=Teclados%2C+Mouse+y+Perif%C3%A9ricos+de+Entrada&i=computers&rh=n%3A11548956011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_89%3AApple%7CDell%7CLenovo%7CLogitech%7CMicrosoft%2Cp_n_deal_type%3A23566065011&dc&language=es&_encoding=UTF8&c=ts&qid=1645624521&rnid=23566063011&ts_id=11548956011&ref=sr_nr_p_n_deal_type_1",
                    "https://www.amazon.com/-/es/s?keywords=Accesorios+para+Monitor+de+Computadora&i=computers&rh=n%3A281062%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_n_deal_type%3A23566065011&dc&language=es&_encoding=UTF8&c=ts&qid=1645624615&rnid=23566063011&ts_id=281062&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?keywords=Tarjetas+de+Memoria&i=computers&rh=n%3A516866%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_n_feature_two_browse-bin%3A13203834011%7C13203835011%7C6518304011%7C6518305011%2Cp_36%3A-30000%2Cp_n_condition-type%3A2224371011%2Cp_89%3AKingston%7CSAMSUNG%2Cp_n_deal_type%3A23566065011&dc&language=es&_encoding=UTF8&c=ts&qid=1645624707&rnid=23566063011&ts_id=516866&ref=sr_nr_p_n_deal_type_1",
                    "https://www.amazon.com/-/es/s?i=computers&bbn=1232597011&rh=n%3A1232597011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A-10000%2Cp_n_condition-type%3A2224371011%2Cp_n_deal_type%3A23566065011&dc&fs=true&language=es&qid=1645624780&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?i=computers&bbn=565098&rh=n%3A565098%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A2421880011%2Cp_n_condition-type%3A2224371011%2Cp_n_deal_type%3A23566065011&dc&fs=true&language=es&qid=1645624869&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?i=computers&bbn=565108&rh=n%3A565108%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A2421886011%2Cp_n_size_browse-bin%3A2423840011%7C2423841011%7C7817234011%2Cp_n_condition-type%3A2224371011%2Cp_89%3AASUS%7CAcer%7CDell%7CHP%7CLenovo%7CSamsung+Electronics%2Cp_n_deal_type%3A23566065011&dc&fs=true&language=es&qid=1645624946&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?keywords=Micr%C3%B3fonos+de+Grabaci%C3%B3n+y+Accesorios&i=mi&rh=n%3A11974521%2Cp_76%3A1249167011%2Cp_72%3A1248939011%2Cp_36%3A-10000%2Cp_n_deal_type%3A23566065011&dc&language=es&_encoding=UTF8&c=ts&qid=1645625078&rnid=403792011&ts_id=11974521&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/-/es/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_72%3A2491149011%2Cp_76%3A2491146011%2Cp_36%3A-30000%2Cp_n_condition-type%3A6503240011%2Cp_n_deal_type%3A23566065011&dc&fs=true&language=es&qid=1645625128&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?i=electronics&bbn=13447451&rh=n%3A13447451%2Cp_72%3A1248879011%2Cp_76%3A1249137011%2Cp_36%3A-30000%2Cp_n_condition-type%3A2224371011%2Cp_n_deal_type%3A23566065011&dc&fs=true&language=es&qid=1645625208&rnid=23566063011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?keywords=Accesorios+de+Cine+En+Casa%2C+TV+y+Video&i=electronics&rh=n%3A3230976011%2Cp_n_condition-type%3A2224371011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A-30000%2Cp_n_deal_type%3A23566065011&dc&language=es&_encoding=UTF8&c=ts&qid=1645625259&rnid=23566063011&ts_id=3230976011&ref=sr_nr_p_n_deal_type_2",
                    "https://www.amazon.com/-/es/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-30000%2Cp_n_deal_type%3A23566065011&dc&fs=true&language=es&qid=1645625307&rnid=23566063011&ref=sr_nr_p_n_deal_type_2"};
        
        static string[] ebayLinkList = { "https://www.ebay.com/b/Cell-Phones-Smartphones/9355?Brand=HTC%7CHuawei%7CAlcatel%7CAmazon%7CApple%7CBLU%7CGoogle%7CLenovo%7CLG%7CMotorola%7CNokia%7CSamsung%7CRedmi%7CT%252DMobile%7CXiaomi%7CZTE&Connectivity=4G%7C4G%252B%7C5G%7CLTE&LH_BIN=1&LH_FS=1&LH_ItemCondition=1000&Operating%2520System=Android%7CiOS&RAM=2%2520GB%7C3%2520GB%7C16%2520GB%7C12%2520GB%7C8%2520GB%7C6%2520GB%7C4%2520GB&Screen%2520Size=5%252E5%2520%252D%25205%252E9%2520in%7C6%2520in%2520or%2520More&Storage%2520Capacity=128%2520GB%7C512%2520GB%7C64%2520GB%7C32%2520GB%7C256%2520GB&mag=1&rt=nc&_fsrp=0&_pgn=1&_sacat=9355&_udhi=120",
                    "https://www.ebay.com/b/TVs/11071?Brand=Haier%7CRCA%7CSamsung%7CSony%7CLG%7CPhilips%7CTCL&Display%2520Technology=LCD%7CLED%7COLED%7CPlasma%7CQLED&LH_BIN=1&LH_FS=1&LH_ItemCondition=1000&Screen%2520Size=20%252D29%2520in%7C30%252D39%2520in%7C40%252D49%2520in%7C50%252D59%2520in%7C60%252D69%2520in%7C70%252D80%2520in&mag=1&rt=nc&_fsrp=0&_pgn=1&_sacat=11071&_udhi=300",
                    "https://www.ebay.com/b/PC-Laptops-Netbooks/177?Brand=Acer%7CIntel%7CLenovo%7CSamsung%7CSony%7CHP%7CDell%7CASUS%7CApple%7CMicrosoft&LH_BIN=1&LH_FS=1&LH_ItemCondition=1000&Screen%2520Size=15%252D15%252E9%2520in%7C16%252D16%252E9%2520in&mag=1&rt=nc&_fsrp=0&_pgn=1&_sacat=177&_udhi=320"};

        static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"/logs";

        static async Task Main(string[] args)
        {
            var listItems = new List<SP_GetAllLinks_Result>();
            

            try
            {
                var task1 = Task.Run(async () =>
                 {   
                    
                     foreach (string link in amazonLinkList)
                     {
                        /* await Selenium(link);
                         amazonLinkPositon++;*/
                     }
                 });


                var task2 = Task.Run(async () =>
                {
                    //await Selenium("https://thestore.com/c/refurbished-cell-phones-58");
                });

                var task3 = Task.Run(async () =>
                {
                    foreach (string link in ebayLinkList)
                    {
                        await Selenium(link);
                        ebayLinkPositon++;
                    }
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
                            context.SP_UPDATE_STATUS(i.id, true);
                        }
                        else
                        {
                             context.SP_UPDATE_STATUS(i.id, false);
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


        private async static Task Selenium(String url)
        {
            await Task.Run(() => {
                ChromeOptions options = new ChromeOptions();
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
                options.AddArguments("disable-infobars", "--no-sandbox", "--disable-dev-shm-usage", " --lang=en-us", $"--user-agent={userAgent}",
                      "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors",
                      "--window-size=1920,1080", "--disable-browser-side-navigation", "--headless");
                options.AddExcludedArgument("enable-automation");

                if (url.Contains("amazon.com"))
                {
                    new Amazon(url, options);
                }
                else if (url.Contains("thestore.com"))
                {
                    new TheStore(url, options);
                }
                else if (url.Contains("ebay.com"))
                {
                    new Ebay(url, options);
                }
            });
            
        }
         
        public static async Task SaveOrUpdate(bool save, string name, string link, string image, decimal price, int condition,string shop)
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                var data = await context.Items.FirstOrDefaultAsync(i=> i.LINK == link);
                checkList.Add(link);

                if (data != null)
                {
                    decimal oldPrice = (decimal) ((decimal) data.OLD_PRICE >0? data.OLD_PRICE: data.PRICE);
                    decimal saving =0, saving_percent= 0;
                    bool validate = true;

                    if (oldPrice > price)
                    {
                       saving = oldPrice - price;
                       saving_percent = saving / oldPrice * 100;
                    }
                    else if (oldPrice < price)
                    {
                        oldPrice = 0;
                    }
                    else { validate = false; }

                    if (validate)
                    {
                        context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                        context.SP_UPDATE_PRICE(data.ID, price, oldPrice, saving, saving_percent);
                        await context.SaveChangesAsync();
                    }
                    
                    
                }
                else if (save)
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                    context.SP_ADD(name, price, link, condition, shop, image);
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
            
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(logPath)) { using (FileStream fs = new FileStream(logPath, FileMode.Create)) ; }
           
            waitHandle.WaitOne();
            var sw = new StreamWriter(logPath, true);
            await sw.WriteLineAsync(log);
            sw.Close();
            waitHandle.Set();
        }

    }
}
