using System;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public interface IDeduplicator
    {
        ResponseMessage GetExistingResponseMessage(Guid requestId);
        void StoreResponseToMessage(Guid requestId, ResponseMessage response);
    }
}