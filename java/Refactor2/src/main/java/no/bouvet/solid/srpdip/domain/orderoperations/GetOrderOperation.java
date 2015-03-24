package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.OrderRepository;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessageFactory;

public class GetOrderOperation implements OrderOperation {

	public static Factory<GetOrderOperation> factory = new Factory<>(GetOrderOperation.class);

	private OrderRepository orderRepository = OrderRepository.factory.getInstance();
	private ResponseMessageFactory responseMessageFactory = ResponseMessageFactory.factory.getInstance();

	@Override
	public ResponseMessage execute(RequestMessage request) {
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
