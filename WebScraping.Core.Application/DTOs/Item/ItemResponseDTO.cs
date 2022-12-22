namespace WebScraping.Core.Application.DTOs.Item
{
    public class ItemResponseDTO
    {
        public string Id { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int ConditionId { get; set; }
        public int ShopId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }

    }
}
