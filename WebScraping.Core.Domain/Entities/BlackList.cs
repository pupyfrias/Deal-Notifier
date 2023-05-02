﻿using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class BlackList : AuditableBaseEntity
    {
        public int Id { get; }
        public string Link { get; set; }
    }
}