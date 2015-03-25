using System.Collections.Generic;
using System.Linq;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class OrderOperationFactory : IOrderOperationFactory
    {
        private readonly Dictionary<Operation, IOrderOperation> _availableOperations;

        public OrderOperationFactory(IEnumerable<IOrderOperation> availableOperations)
        {
            _availableOperations = availableOperations.ToDictionary(o => o.Operation, o => o);
        }

        public IOrderOperation GetOrderOperation(Operation operation)
        {
            IOrderOperation operationObject;

            return _availableOperations.TryGetValue(operation, out operationObject) 
                ? operationObject 
                : null;
        }
    }
}
