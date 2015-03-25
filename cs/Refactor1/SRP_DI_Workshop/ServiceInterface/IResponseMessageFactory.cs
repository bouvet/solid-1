using System;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop.ServiceInterface
{
    public interface IResponseMessageFactory
    {
        ResponseMessage CreateCancellationResponseMessage(RequestMessage reqMsg, Order orderToCancel, string message);
        ResponseMessage CreateOrderSubmissionResponseMessage(RequestMessage reqMsg, Order order);
        ResponseMessage CreateOrderQueryResponseMessage(RequestMessage reqMsg, Order orderToGet);
        ResponseMessage CreateErrorResponseMessage(Exception ex);
        ResponseMessage CreateBadRequestResponseMessage(RequestMessage reqMsg);
    }
}