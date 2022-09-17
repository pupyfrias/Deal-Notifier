namespace WebScraping.Models
{
    public struct Item
    {
        public int Condition { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Save { get; set; }
        public  int Shop { get; set; }
       public  int Type { get; set; }

    }
}
