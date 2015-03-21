using System;

namespace SRP_DI_Workshop.ServiceInterface
{
    [Serializable]
    public class OrderCancellationResponse
    {
        public Guid RequestId { get; set; }
        public long OrderId { get; set; }
        public string OrderState { get; set; }
        public string Message { get; set; }
    }
}
