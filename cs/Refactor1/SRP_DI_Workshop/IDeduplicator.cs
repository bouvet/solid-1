using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public interface IDeduplicator
    {
        ResponseMessage GetExistingResponseMessage(long requestId);
        void StoreResponseToMessage(long requestId, ResponseMessage response);
    }
}