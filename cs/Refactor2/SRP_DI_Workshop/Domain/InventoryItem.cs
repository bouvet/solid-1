using System;

namespace SRP_DI_Workshop.Domain
{
    [Serializable]
    public class InventoryItem
    {
        public string ItemCode { get; set; }
        public string PricingCalculation { get; set; }
        public string UnitName { get; set; }
        public float WeightPerUnit { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal RebatePerUnit { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal VatRate { get; set; }
    }
}