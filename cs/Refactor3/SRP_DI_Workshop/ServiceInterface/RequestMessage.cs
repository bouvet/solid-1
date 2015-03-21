using System;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop.ServiceInterface
{
    [Serializable]
    public class RequestMessage
    {
        public Guid RequestId { get; set; }
        public Operation Operation { get; set; }
        public long AccountId { get; set; }
        public long OrderId { get; set; }
        public OrderItem[] OrderItems { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public string ContactEmail { get; set; }
    }
}
