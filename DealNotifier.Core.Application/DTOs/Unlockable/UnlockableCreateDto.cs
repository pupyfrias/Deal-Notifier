﻿namespace DealNotifier.Core.Application.DTOs.Unlockable
{
    public class UnlockableCreateDto
    {
        public int BrandId { get; set; }
        public string Comment { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
    }
}