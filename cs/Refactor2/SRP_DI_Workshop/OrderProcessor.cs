using SRP_DI_Workshop.Domain;
using SRP_DI_Workshop.Domain.OrderOperations;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class OrderProcessor
    {
        private readonly IResponseMessageFactory _responseMessageFactory;
        private readonly ILoggerService _loggerService;
        private readonly IDeduplicator _deduplicator;
        private readonly IDependencyResolver _dependencyResolver;

        public OrderProcessor(
            IResponseMessageFactory responseMessageFactory,
            ILoggerService loggerService,
            IDeduplicator deduplicator,
            IDependencyResolver dependencyResolver)
        {
            _responseMessageFactory = responseMessageFactory;
            _loggerService = loggerService;
            _deduplicator = deduplicator;
            _dependencyResolver = dependencyResolver;
        }

        public ResponseMessage ProcessOrder(RequestMessage reqMsg)
        {
            ResponseMessage responseToDuplicateMessage = _deduplicator.GetExistingResponseMessage(reqMsg.RequestId);

            if (responseToDuplicateMessage != null)
                return responseToDuplicateMessage;

            ResponseMessage response;

            switch (reqMsg.Operation)
            {
                case CreateOrderOperation.Id:
                    CreateOrderOperation submitOrder = _dependencyResolver.Resolve<CreateOrderOperation>();
                    response = submitOrder.ExecuteOperation(reqMsg);
                    break;
                case CancelOrderOperation.Id:
                    CancelOrderOperation cancelOrder = _dependencyResolver.Resolve<CancelOrderOperation>();
                    response = cancelOrder.ExecuteOperation(reqMsg);
                    break;
                case GetOrderOperation.Id:
                    GetOrderOperation getOrder = _dependencyResolver.Resolve<GetOrderOperation>();
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
