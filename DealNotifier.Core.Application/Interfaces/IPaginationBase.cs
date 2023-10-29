namespace Catalog.Application.Interfaces
{
    public interface IPaginationBase
    {
        string? OrderBy { get; }
        bool Descending { get; }
        int Offset { get; }
        int Limit { get; }
    }
}