using SRP_DI_Workshop.Domain.OrderOperations;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class OrderProcessor
    {
        private readonly IResponseMessageFactory _responseMessageFactory;
        private readonly ILoggerService _loggerService;
        private readonly IDeduplicator _deduplicator;
        private readonly IOrderOperationFactory _orderOperationFactory;

        public OrderProcessor(
            IResponseMessageFactory responseMessageFactory,
            ILoggerService loggerService,
            IDeduplicator deduplicator,
            IOrderOperationFactory orderOperationFactory)
        {
            _responseMessageFactory = responseMessageFactory;
            _loggerService = loggerService;
            _deduplicator = deduplicator;
            _orderOperationFactory = orderOperationFactory;
        }

        public ResponseMessage ProcessOrder(RequestMessage reqMsg)
        {
            ResponseMessage responseToDuplicateMessage = _deduplicator.GetExistingResponseMessage(reqMsg.RequestId);

            if (responseToDuplicateMessage != null)
                return responseToDuplicateMessage;

            IOrderOperation orderOperation = _orderOperationFactory.GetOrderOperation(reqMsg.Operation);

            ResponseMessage response;

            if (orderOperation != null)
                response = orderOperation.ExecuteOperation(reqMsg);
            else
            {
                _loggerService.WriteLine("Received bad message: " + reqMsg.RequestId, "ProcessOrderError");
                response = _responseMessageFactory.CreateBadRequestResponseMessage(reqMsg);
            }

            _deduplicator.StoreResponseToMessage(reqMsg.RequestId, response);

            return response;
        }
    }
}
