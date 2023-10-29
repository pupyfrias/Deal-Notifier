namespace ApiGateway.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entityName, object key) : base($"{entityName} with id ({key.ToString()}) was not found")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}