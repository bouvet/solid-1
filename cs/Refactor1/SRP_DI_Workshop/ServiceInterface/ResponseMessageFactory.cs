using System;
using System.Net;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop.ServiceInterface
{
    public class ResponseMessageFactory
    {
        public ResponseMessage CreateCancellationResponseMessage(RequestMessage reqMsg, Order orderToCancel, string message)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new OrderCancellationResponse
                {
                    RequestId = reqMsg.RequestId,
                    OrderId = reqMsg.OrderId,
                    OrderState = orderToCancel.State.ToString(),
                    Message = message
                }
            };
        }

        public ResponseMessage CreateOrderSubmissionResponseMessage(RequestMessage reqMsg, Order order)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new OrderSubmissionResponse
                {
                    RequestId = reqMsg.RequestId,
                    OrderId = order.OrderId,
                    OrderState = order.State.ToString()
                }
            };
        }

        public ResponseMessage CreateOrderQueryResponseMessage(RequestMessage reqMsg, Order orderToGet)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new OrderQueryResponseMessage
                {
                    RequestId = reqMsg.RequestId,
                    Order = orderToGet
                }
            };
        }

        public ResponseMessage CreateErrorResponseMessage(Exception ex)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseBody = ex.ToString()
            };
        }

        public ResponseMessage CreateBadRequestResponseMessage(RequestMessage reqMsg)
        {
            return new ResponseMessage 
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseBody = reqMsg 
            };
        }
    }
}
