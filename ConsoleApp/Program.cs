using DataBase;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ConsoleApp
{
    class Program
    {
        public static int amazonLinkPositon = 1;
        public static ArrayList checkList = new ArrayList();
        public static string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        public static string[] filterList = {"tracfone", "total wireless", "net10", "simple mobile", "straight talk"};
        public static string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };
        public static string[] amazonLinkList = {"https://www.amazon.com/-/es/s?i=computers&bbn=17923671011&rh=n%3A17923671011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A-20000%2Cp_n_condition-type%3A2224371011%2Cp_89%3AARCTIC%7CCorsair%7CCrucial%7CElgato%7CNoctua%7CSamsung+Electronics%7CSeagate&dc&language=es&brr=1&pd_rd_r=d14c0261-e650-46e8-a548-15bac2fa9b3d&pd_rd_w=8M1il&pd_rd_wg=cdnkB&qid=1644022815&rd=1&rnid=2528832011&ref=sr_nr_p_89_7",
                    "https://www.amazon.com/-/es/s?keywords=Teclados%2C+Mouse+y+Perif%C3%A9ricos+de+Entrada&i=computers&rh=n%3A11548956011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_89%3AApple%7CDell%7CLenovo%7CLogitech%7CMicrosoft&dc&language=es&_encoding=UTF8&c=ts&qid=1644022401&rnid=2528832011&ts_id=11548956011&ref=sr_nr_p_89_5",
                    "https://www.amazon.com/-/es/s?keywords=Accesorios+para+Monitor+de+Computadora&i=computers&rh=n%3A281062%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011&dc&language=es&_encoding=UTF8&c=ts&qid=1644022172&rnid=2224369011&ts_id=281062&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/-/es/s?keywords=Tarjetas+de+Memoria&i=computers&rh=n%3A516866%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_n_feature_two_browse-bin%3A13203834011%7C13203835011%7C6518304011%7C6518305011%2Cp_36%3A-30000%2Cp_n_condition-type%3A2224371011%2Cp_89%3AKingston%7CSAMSUNG&dc&language=es&_encoding=UTF8&c=ts&qid=1644021993&rnid=2528832011&ts_id=516866&ref=sr_nr_p_89_1",
                    "https://www.amazon.com/-/es/s?i=computers&bbn=1232597011&rh=n%3A1232597011%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A-10000%2Cp_n_condition-type%3A2224371011&dc&fs=true&language=es&qid=1644021090&rnid=2224369011&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/-/es/s?i=computers&bbn=565098&rh=n%3A565098%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A2421880011%2Cp_n_condition-type%3A2224371011&dc&fs=true&language=es&qid=1644020936&rnid=2224369011&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/-/es/s?i=computers&bbn=565108&rh=n%3A565108%2Cp_76%3A1249137011%2Cp_72%3A1248879011%2Cp_36%3A2421886011%2Cp_n_size_browse-bin%3A2423840011%7C2423841011%7C7817234011%2Cp_n_condition-type%3A2224371011%2Cp_89%3AASUS%7CAcer%7CDell%7CHP%7CLenovo%7CSamsung+Electronics&dc&fs=true&language=es&qid=1644020655&rnid=2528832011&ref=sr_nr_p_89_7",
                    "https://www.amazon.com/-/es/s?keywords=Micr%C3%B3fonos+de+Grabaci%C3%B3n+y+Accesorios&i=mi&rh=n%3A11974521%2Cp_76%3A1249167011%2Cp_72%3A1248939011%2Cp_36%3A-10000%2Cp_n_condition-type%3A404228011&dc&language=es&_encoding=UTF8&c=ts&qid=1644020103&rnid=403792011&ts_id=11974521&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/-/es/s?i=mobile&bbn=7072561011&rh=n%3A7072561011%2Cp_72%3A2491149011%2Cp_76%3A2491146011%2Cp_36%3A-30000%2Cp_n_condition-type%3A6503240011&dc&fs=true&language=es&qid=1644018986&rnid=6503239011&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/-/es/s?i=electronics&bbn=13447451&rh=n%3A13447451%2Cp_72%3A1248879011%2Cp_76%3A1249137011%2Cp_36%3A-30000%2Cp_n_condition-type%3A2224371011&dc&fs=true&language=es&qid=1644018847&rnid=2224369011&ref=sr_nr_p_n_condition-type_1",
                    "https://www.amazon.com/s?k=Accesorios+de+Cine+En+Casa%2C+TV+y+Video&i=electronics&rh=n%3A3230976011%2Cp_n_condition-type%3A2224371011%2Cp_76%3A1249137011%2Cp_72%3A1248879011&dc=&language=es&_encoding=UTF8&c=ts&qid=1644018776&rnid=386442011&ts_id=3230976011&ref=sr_nr_p_36_3&low-price=&high-price=300",
                    "https://www.amazon.com/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_72%3A1248879011%2Cp_n_condition-type%3A2224371011%2Cp_36%3A-30000&dc&fs=true&language=es&qid=1644018448&rnid=8589203011&ref=sr_nr_p_36_5"};
        
        static string[] ebayLinkList = { "https://www.ebay.com/b/Cell-Phones-Smartphones/9355?mag=1&_fsrp=0&LH_FS=1&rt=nc&LH_ItemCondition=1000&Brand=HTC%7CHuawei%7CAlcatel%7CAmazon%7CApple%7CBLU%7CGoogle%7CLenovo%7CLG%7CMotorola%7CNokia%7CSamsung%7CRedmi%7CT%252DMobile%7CXiaomi%7CZTE&LH_BIN=1&_sacat=9355&Storage%2520Capacity=128%2520GB%7C512%2520GB%7C64%2520GB%7C32%2520GB%7C256%2520GB&_udhi=220" };


        static async Task Main(string[] args)
        {
            var listItems = new List<SP_GetAllLinks_Result>();
            try
            {
                var task1 = Task.Run(async () =>
                 {   
                    
                     foreach (string link in amazonLinkList)
                     {
                         await Selenium(link);
                         amazonLinkPositon++;
                     }
                 });


                var task2 = Task.Run(async () =>
                {
                    await Selenium("https://thestore.com/c/refurbished-cell-phones-58");
                });

                var task3 = Task.Run(async () =>
                {
                    foreach (string link in ebayLinkList)
                    {
                        await Selenium(link);
                        //linkPositon++;
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
                    if (checkList.Contains(i.link))
                    {
                        using (WebScrapingEntities context = new WebScrapingEntities())
                        {
                            context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                            context.SP_UPDATE_STATUS(i.id, true);
                        }
                    }
                    else
                    {
                        using (WebScrapingEntities context = new WebScrapingEntities())
                        {
                            context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
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
                options.AddArguments("start-maximized", "disable-infobars", "--no-sandbox", "--disable-dev-shm-usage",
                      "--disable-gpu", "--disable-extensions", "--allow-running-insecure-content", "--ignore-certificate-errors","headless");

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
    }
}
