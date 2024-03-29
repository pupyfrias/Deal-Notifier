﻿namespace DealNotifier.Infrastructure.EbayDataSyncWorker.ViewModels
{
    public class ShippingOption
    {
        public bool GuaranteedDelivery { get; set; }
        public string MaxEstimatedDeliveryDate { get; set; }
        public string MinEstimatedDeliveryDate { get; set; }
        public ShippingCost ShippingCost { get; set; }
        public string ShippingCostType { get; set; }
    }
}