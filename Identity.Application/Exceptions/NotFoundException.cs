using Grpc.Core;

namespace Identity.Application.Exceptions
{
    public class NotFoundException : RpcException
    {
        public NotFoundException(string message) : base(new Status(StatusCode.NotFound, message))
        {
        }

        public NotFoundException(string entityName, object id): base
            (new Status(StatusCode.NotFound, $"{entityName} with Id {id.ToString()} was not found."))
        {
        }

    }
}