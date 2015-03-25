using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public interface IOrderOperation
    {
        Operation Operation { get; }
        ResponseMessage ExecuteOperation(RequestMessage request);
    }
}