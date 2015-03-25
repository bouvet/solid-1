using System;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop.ServiceInterface
{
    [Serializable]
    public class OrderQueryResponseMessage
    {
        public long RequestId { get; set; }
        public Order Order { get; set; }
    }
}
