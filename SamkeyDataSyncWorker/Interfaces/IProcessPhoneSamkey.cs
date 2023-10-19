namespace SamkeyDataSyncWorker.Interfaces
{
    public interface IProcessPhoneSamkey
    {
        Task ProcessAsync(List<string> phoneList);
    }
}