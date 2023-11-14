namespace DealNotifier.Core.Application.ViewModels.eBay
{
    public class EBayErrorResponse
    {
        public List<Error> Errors { get; set; } = new();
       
    }

    public class Error
    {
        public int ErrorId { get; set; }
        public string Domain { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
    }

}