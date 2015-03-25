using System.Collections.Generic;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class Deduplicator : IDeduplicator
    {
        private readonly Dictionary<long, ResponseMessage> _processedMessages = new Dictionary<long, ResponseMessage>();

        public ResponseMessage GetExistingResponseMessage(long requestId)
        {
            ResponseMessage responseToPreviousMessage;

            return _processedMessages.TryGetValue(requestId, out responseToPreviousMessage) 
                ? responseToPreviousMessage 
                : null;
        }

        public void StoreResponseToMessage(long requestId, ResponseMessage response)
        {
            _processedMessages[requestId] = response;
        }
    }
}