package no.bouvet.solid.srpdip.messageinterface;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.domain.Order;

public class ResponseMessageFactory {

	public static Factory<ResponseMessageFactory> factory = new Factory<>(ResponseMessageFactory.class);

	public ResponseMessage createOrderQueryResponseMessage(RequestMessage reqMsg, Order orderToGet)
	{
		return new OrderQueryResponseMessage(reqMsg.getRequestId(), orderToGet);
	}

	public ResponseMessage createCancellationResponseMessage(RequestMessage reqMsg, Order orderToCancel, String message)
	{
		return new OrderCancellationResponse(
				reqMsg.getRequestId(),
				reqMsg.getOrderId(),
				orderToCancel.getState().toString(),
				message);
	}

	public ResponseMessage createErrorResponseMessage(long requestId, Exception ex)
	{
		return new ResponseMessage(requestId);
	}

	public ResponseMessage createOrderSubmissionResponseMessage(RequestMessage reqMsg, Order order)
	{
		return new OrderSubmissionResponse(
				reqMsg.getRequestId(),
				order.getOrderId(),
				order.getState().toString());
	}
}
