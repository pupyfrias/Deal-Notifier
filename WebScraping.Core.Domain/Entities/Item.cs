namespace WebScraping.Core.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Saving { get; set; }
        public string Link { get; set; }
        public int ConditionId { get; set; }
        public int StatusId { get; set; }
        public int ShopId { get; set; }
        public string Image { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int TypeId { get; set; }
        public Condition Condition { get; set; }
        public Status Status { get; set; }
        public Shop Shop { get; set; }
        public Type Type { get; set; }


    }
}
