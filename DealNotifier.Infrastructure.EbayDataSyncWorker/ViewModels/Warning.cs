﻿namespace DealNotifier.Infrastructure.EbayDataSyncWorker.ViewModels
{
    public class Warning
    {
        public string Category { get; set; }
        public string Domain { get; set; }
        public int ErrorId { get; set; }
        public List<string> InputRefIds { get; set; }
        public string LongMessage { get; set; }
        public string Massage { get; set; }
        public List<string> OutputRefIds { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string Subdomain { get; set; }
    }
}