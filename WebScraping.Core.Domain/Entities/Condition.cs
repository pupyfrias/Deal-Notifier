﻿using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Condition : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<ConditionsToNotify> ConditionToNotifies { get; set; }
    }
}