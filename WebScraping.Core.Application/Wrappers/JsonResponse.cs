namespace WebScraping.Core.Application.Wrappers
{
    public class Response<T>
    {
        public Response(T data, string message = "Success Request")
        {
            Data = data;
            Message = message;
            Success = true;

        }

        public Response(string message)
        {
            Message = message;
            Success=false;
        }

        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message  { get; set; }
        public List<string> Errors { get; set; }


    }
}
