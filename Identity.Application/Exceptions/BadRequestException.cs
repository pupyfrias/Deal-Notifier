using Grpc.Core;

namespace Identity.Application.Exceptions
{
    public class BadRequestException : RpcException
    {
        public BadRequestException(string message) : base(new Status(StatusCode.InvalidArgument, message))
        {
        }
    }
}