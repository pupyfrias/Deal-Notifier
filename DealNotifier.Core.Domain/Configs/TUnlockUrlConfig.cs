namespace DealNotifier.Core.Domain.Configs
{
    public class TUnlockUrlConfig
    {
        public string Base { get; set; }
        public IEnumerable<string> Paths { get; set; }
    }
}