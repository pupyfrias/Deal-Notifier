﻿using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Supported: AuditableBaseEntity
    {
        public string ModelNumber { get; set; }
        public string ModelName { get; set; }
        public string Tool { get; set; }
        public  string SupportedVersion{ get; set; }
        public  string SupportedBit{ get; set; }
        public  string Carrier { get; set; }
        public  string Comment { get; set; }

    }
}