namespace Catalog.Application.ViewModels.V1.UnlockabledPhone
{
    public class UnlockabledPhoneResponse
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string? Comment { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
    }
}