using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public interface IOrderOperationFactory
    {
        IOrderOperation GetOrderOperation(Operation operation);
    }
}
