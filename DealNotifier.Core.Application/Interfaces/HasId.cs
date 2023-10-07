namespace DealNotifier.Core.Application.Interfaces
{
    public interface IHasId<TKey>
    {
        TKey Id { get; set; }
    }

}
