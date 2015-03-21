using System;

namespace SRP_DI_Workshop.Domain
{
    [Serializable]
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public string ItemCode { get; set; }
        public float WeightPerUnit { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal RebatePerUnit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Rebate { get; set; }
        public decimal Vat { get; set; }
        public float Weight { get; set; }
        public OrderItemState State { get; set; }
    }
}