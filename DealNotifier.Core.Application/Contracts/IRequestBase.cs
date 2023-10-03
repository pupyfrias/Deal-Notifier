namespace DealNotifier.Core.Application.Contracts
{
    public interface IRequestBase
    {
        string? OrderBy { get; }
        bool Descending { get; }
        int Offset { get; }
        int Limit { get; }
    }
}
