using System;
using System.Collections.Generic;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class Deduplicator : IDeduplicator
    {
        private readonly Dictionary<Guid, ResponseMessage> _processedMessages = new Dictionary<Guid, ResponseMessage>();

        public ResponseMessage GetExistingResponseMessage(Guid requestId)
        {
            ResponseMessage responseToPreviousMessage;

            return _processedMessages.TryGetValue(requestId, out responseToPreviousMessage) 
                ? responseToPreviousMessage 
                : null;
        }

        public void StoreResponseToMessage(Guid requestId, ResponseMessage response)
        {
            _processedMessages[requestId] = response;
        }
    }
}