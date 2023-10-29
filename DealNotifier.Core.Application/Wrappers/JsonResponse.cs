namespace Catalog.Application.Wrappers
{
    public class Response<T>
    {
        public Response(T data, string message = "Succussed Request")
        {
            Data = data;
            Message = message;
            Succussed = true;
        }

        public Response(string message)
        {
            Message = message;
            Succussed = false;
        }

        public T Data { get; set; }
        public bool Succussed { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}