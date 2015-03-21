using SRP_DI_Workshop.Domain.OrderOperations;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class OrderProcessor
    {
        private readonly Deduplicator _deduplicator = new Deduplicator();
        private readonly InventoryRepository _InventoryRepository = new InventoryRepository();
        private readonly OrderRepository _orderRepository = new OrderRepository();
        private readonly LoggerService _loggerService = new LoggerService();
        private readonly ResponseMessageFactory _responseMessageFactory = new ResponseMessageFactory();

        public ResponseMessage ProcessOrder(RequestMessage reqMsg)
        {
            ResponseMessage responseToDuplicateMessage = _deduplicator.GetExistingResponseMessage(reqMsg.RequestId);

            if (responseToDuplicateMessage != null)
                return responseToDuplicateMessage;

            ResponseMessage response;

            switch (reqMsg.Operation)
            {
                case CreateOrderOperation.Id:
                    CreateOrderOperation submitOrder = new CreateOrderOperation
                    {
                        InventoryRepository = _InventoryRepository,
                        OrderRepository = _orderRepository
                    };

                    response = submitOrder.ExecuteOperation(reqMsg);
                    break;
                case CancelOrderOperation.Id:
                    CancelOrderOperation cancelOrder = new CancelOrderOperation
                    {
                        InventoryRepository = _InventoryRepository,
                        OrderRepository = _orderRepository
                    };

                    response = cancelOrder.ExecuteOperation(reqMsg);
                    break;
                case GetOrderOperation.Id:
                    GetOrderOperation getOrder = new GetOrderOperation
                    {
                        OrderRepository = _orderRepository
                    };

                    response = getOrder.ExecuteOperation(reqMsg);
                    break;
                default:
                    _loggerService.WriteLine("Received bad message: " + reqMsg.RequestId, "ProcessOrderError");
                    response = _responseMessageFactory.CreateBadRequestResponseMessage(reqMsg);
                    break;
            }

            _deduplicator.StoreResponseToMessage(reqMsg.RequestId, response);

            return response;
        }
    }
}
