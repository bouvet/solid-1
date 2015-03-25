using System;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class GetOrderOperation : IOrderOperation
    {
        public OrderRepository OrderRepository;
        private readonly LoggerService _loggerService = new LoggerService();
        private readonly ResponseMessageFactory _responseMessageFactory = new ResponseMessageFactory();

        public const Operation Id = Operation.Get_Order_Details;

        public ResponseMessage ExecuteOperation(RequestMessage request)
        {
            try
            {
                Order orderToGet = OrderRepository.GetOrder(request.OrderId);

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