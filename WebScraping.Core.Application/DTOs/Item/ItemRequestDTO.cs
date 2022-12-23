namespace WebScraping.Core.Application.DTOs.Item
{
    public class ItemRequestDTO
    {
        public string? brands { get; set; }
        public string? storages { get; set; }
        public string? search { get; set; }
        public string? max { get; set; }
        public string? min { get; set; }
        public string? sort_by { get; set; }
        public string? offer { get; set; }
        public string? types { get; set; }
        public string? condition { get; set; }
        public string? carriers { get; set; }
        public string? excludes { get; set; }
        public string? shops { get; set; }
    }
}