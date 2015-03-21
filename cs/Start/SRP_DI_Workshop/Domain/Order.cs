using System;
using System.Collections.Generic;

namespace SRP_DI_Workshop.Domain
{
    [Serializable]
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public long OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public float TotalWeight { get; set; }
        public decimal RebateTotal { get; set; }
        public decimal ItemSubtotal { get; set; }
        public decimal FreightSubtotal { get; set; }
        public decimal VatSubtotal { get; set; }
        public decimal OrderTotal { get; set; }
        public OrderState State { get; set; }
    }
}