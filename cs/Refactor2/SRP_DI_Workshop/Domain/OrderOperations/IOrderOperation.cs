using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public interface IOrderOperation
    {
        ResponseMessage ExecuteOperation(RequestMessage request);
    }
}