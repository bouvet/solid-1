package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.OrderRepository;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessageFactory;

public class GetOrderOperation implements OrderOperation {

	private OrderRepository orderRepository;

	private ResponseMessageFactory responseMessageFactory = new ResponseMessageFactory();

	public GetOrderOperation(OrderRepository orders) {
		this.orderRepository = orders;
	}

	@Override
	public ResponseMessage executeOperation(RequestMessage request) {
		try {
			Order orderToGet = orderRepository.orders.get(request.getOrderId());

			return responseMessageFactory.createOrderQueryResponseMessage(request, orderToGet);
		} catch (Exception ex) {
			// Trace.WriteLine("Exception during operation GetOrderDetails: " +
			// ex, "GetOrderDetailsError");
			return responseMessageFactory.createErrorResponseMessage(request.getRequestId(), ex);
		}
	}

}
