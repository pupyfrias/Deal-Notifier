using Catalog.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.Item
{
    public class ItemUpdateRequest : IHasId<Guid>
    {
        public int BidCount { get; set; }
        [Required]
        public int? ConditionId { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Image { get; set; }
        public bool IsAuction { get; set; }
        public DateTime? ItemEndDate { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Notify { get; set; }
        public decimal OldPrice { get; set; }
        public int PhoneCarrierId { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        [Required]
        public int? OnlineStoreId { get; set; }
        [Required]
        public int? StockStatusId { get; set; }
        [Required]
        public int? ItemTypeId { get; set; }
        [Required]
        public int? UnlockProbabilityId { get; set; }
    }
}