using System;

namespace SRP_DI_Workshop.ServiceInterface
{
    [Serializable]
    public class OrderSubmissionResponse
    {
        public Guid RequestId { get; set; }
        public long OrderId { get; set; }
        public string OrderState { get; set; }
    }
}
