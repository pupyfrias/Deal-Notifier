using ConsoleApp.Classes;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleApp
{
    class Program: Method
    {
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
                                                  {"https://www.amazon.com/s?i=electronics&bbn=172659&rh=n%3A172659%2Cp_n_condition-type%3A2224371011%2Cp_72%3A1248879011%2Cp_n_specials_match%3A21213697011&dc&qid=1648597881&rnid=21213696011&ref=sr_nr_p_n_specials_match_1",2},
                                                  {"https://www.amazon.com/-/es/s?k=memory+usb&i=electronics&rh=n%3A3151491%2Cp_89%3AKingston%7CSAMSUNG%7CSanDisk%2Cp_72%3A1248879011%2Cp_n_size_browse-bin%3A10285016011%7C10285018011%7C10285020011%7C1259716011%2Cp_n_condition-type%3A2224371011&dc&language=es&__mk_es_US=%C3%85M%C3%85%C5%BD%C3%95%C3%91&crid=3KX7CUPAFF5EL&qid=1650053451&rnid=2224369011&sprefix=memory+usb%2Celectronics%2C187&ref=sr_nr_p_n_condition-type_1",3} };


        static object[,] ebayLinkList = {{"https://www.ebay.com/sch/i.html?_dcat=9355&_fsrp=1&_blrs=recall_filtering&_from=R40&LH_TitleDesc=0&LH_ItemCondition=1000&Brand=Samsung%7CApple%7CMotorola%7CAlcatel%7CXiaomi%7CHuawei%7CLG&_nkw=phones&_sacat=9355&Network=AT%2526T%7CCricket%2520Wireless%7CSprint%7CT%252DMobile%7CUnlocked%7CBoom%2520Mobile%7CBoost%2520Mobile%7CMetro%7CVerizon%7CUS%2520Mobile%7CU%252ES%252E%2520Cellular%7CVirgin%2520Mobile&LH_BIN=1&Storage%2520Capacity=32%2520GB%7C64%2520GB%7C256%2520GB%7C512%2520GB%7C128%2520GB&_sop=15&_udhi=120&rt=nc&_stpos=19720&_sadis=2000&LH_PrefLoc=99&_fspt=1&_ipg=240",1 },
                                        { "https://www.ebay.com/sch/i.html?_dcat=11071&_fsrp=1&rt=nc&_from=R40&LH_PrefLoc=99&LH_ItemCondition=1000&_stpos=19720&_nkw=tv&_sacat=32852&LH_BIN=1&_fspt=1&Screen%2520Size=60%252D69%2520in%7C20%252D29%2520in%7C40%252D49%2520in%7C30%252D39%2520in%7C50%252D59%2520in%7C70%252D80%2520in&_udhi=200&_sadis=2000&_ipg=240" ,2}
                                        };

        static object[,] theStoreLinkList = { { "https://thestore.com/c/refurbished-cell-phones-58", 1 } };
   

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
                    await Selenium("eBay", ebayLinkList);

                });

                await Task.WhenAll(task1, task2, task3);

                using (WebScrapingEntities context = new WebScrapingEntities())
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                    listItems = context.SP_GetAllLinks().ToList();
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


                SystemSounds.Asterisk.Play();
                Console.WriteLine("DONE");
                Thread.Sleep(5000);

            }
            catch (Exception e)
            {
                await WriteLogs(e.Message, "ERROR");

            }

        }


    

    }
}
