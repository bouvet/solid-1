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
		Order orderToGet = orderRepository.getOrder(request.getOrderId());

		return responseMessageFactory.createOrderQueryResponseMessage(request, orderToGet);
	}

}
