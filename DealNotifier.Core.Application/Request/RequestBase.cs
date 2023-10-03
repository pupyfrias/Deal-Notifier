using DealNotifier.Core.Application.Contracts;

namespace DealNotifier.Core.Application.Request
{
    public abstract class RequestBase : IRequestBase
    {
        public string? OrderBy { get; set; }
        public bool Descending { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; } = 10;
    }
}
