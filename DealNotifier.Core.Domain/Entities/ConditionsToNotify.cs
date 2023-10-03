﻿using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class ConditionsToNotify : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
        public Condition Condition { get; set; }
    }
}