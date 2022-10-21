using System.Threading.Tasks;
using WebScraping.Heplers;

namespace WebScraping.Entities
{
    public class Item
    {
        #region Fields
        private readonly string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        private readonly string[] bannedWordList = { "tracfone", "total wireless", "net10", "simple mobile", "straight talk", "family mobile" };
        private readonly string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };
        #endregion Fields

        #region Properties
        public int Condition { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Save { get; set; }
        public int Shop { get; set; }
        public int Type { get; set; }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Set Item Condition
        /// </summary>
        public void SetCondition()
        {
            foreach (string condition in conditionList)
            {
                if (Name.ToLower().Contains(condition))
                {
                    Condition = (int)Emuns.Condition.Used;
                    break;
                }
            }

            Condition = (int)Emuns.Condition.New;
        }
        /// <summary>
        /// Validate if the Item can be save.
        /// </summary>
        /// <returns>true if the Iten is not in BlackList; otherwise, false.</returns>
        public async Task<bool> CanBeSave()
        {
            bool isBanned = false;
            bool isInBlackList = false;

            Task bannedTask = Task.Run(() =>
            {
                foreach (string bannedWord in bannedWordList)
                {
                    if (Name.ToLower().Contains(bannedWord))
                    {
                        isBanned = true;
                        break;
                    }
                }

            });

            Task blackListTask = Task.Run(() =>
            {
                Helper.linkBlackList.Contains(Link);
            });

            await Task.WhenAll(bannedTask, blackListTask);


            if (isBanned || isInBlackList)
            {
                Save = false;
                return false;
            }
            else
            {
                SetCondition();
                Save = true;
                return true;
            }

        }
        #endregion Methods
    }
}
