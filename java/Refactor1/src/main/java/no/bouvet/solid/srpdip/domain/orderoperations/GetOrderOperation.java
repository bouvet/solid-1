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
	public ResponseMessage execute(RequestMessage request) {
		Order orderToGet = orderRepository.getOrder(request.getOrderId());

		return responseMessageFactory.createOrderQueryResponseMessage(request, orderToGet);
	}

}
