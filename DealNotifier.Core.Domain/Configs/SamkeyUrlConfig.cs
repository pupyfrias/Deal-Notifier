namespace DealNotifier.Core.Domain.Configs
{
    public class SamkeyUrlConfig
    {
        public string Base { get; set; }
        public SamkeyPaths Paths { get; set; }
    }

    public class SamkeyPaths
    {
        public string AutoComplete { get; set; }
        public string Supported { get; set; }
    }
}
