using System;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class GetOrderOperation : IOrderOperation
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerService _loggerService;
        private readonly IResponseMessageFactory _responseMessageFactory;

        public const Operation Id = Operation.GetOrderDetails;

        public GetOrderOperation(
            IOrderRepository orderRepository,
            ILoggerService loggerService,
            IResponseMessageFactory responseMessageFactory)
        {
            _orderRepository = orderRepository;
            _loggerService = loggerService;
            _responseMessageFactory = responseMessageFactory;
        }

        public ResponseMessage ExecuteOperation(RequestMessage request)
        {
            try
            {
                Order orderToGet = _orderRepository.Orders[request.OrderId];

                return _responseMessageFactory.CreateOrderQueryResponseMessage(request, orderToGet);
            }
            catch (Exception ex)
            {
                _loggerService.WriteLine("Exception during operation GetOrderDetails: " + ex, "GetOrderDetailsError");
                return _responseMessageFactory.CreateErrorResponseMessage(ex);
            }
        }
    }
}