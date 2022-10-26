namespace WebScraping.Core.Domain.Entities
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
