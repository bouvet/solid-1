package no.bouvet.solid.srpdip;

import no.bouvet.solid.srpdip.domain.orderoperations.CancelOrderOperation;
import no.bouvet.solid.srpdip.domain.orderoperations.CreateOrderOperation;
import no.bouvet.solid.srpdip.domain.orderoperations.GetOrderOperation;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;

public class OrderProcessor {

	private Deduplicator deduplicator = new Deduplicator();
	private InventoryRepository inventoryRepository = new InventoryRepository();
	private OrderRepository orderRepository = new OrderRepository();

	public ResponseMessage processMessage(RequestMessage reqMsg) {
		ResponseMessage responseToDuplicateMessage = deduplicator.getExistingResponseMessage(reqMsg.getRequestId());

		if (responseToDuplicateMessage != null) {
			return responseToDuplicateMessage;
		}

		ResponseMessage response = null;

		switch (reqMsg.getOperation()) {
		case SUBMIT_ORDER:
			response = new CreateOrderOperation(inventoryRepository, orderRepository).execute(reqMsg);
			break;
		case CANCEL_ORDER:
			response = new CancelOrderOperation(inventoryRepository, orderRepository).execute(reqMsg);
			break;
		case GET_ORDER_DETAILS:
			response = new GetOrderOperation(orderRepository).execute(reqMsg);
		}

		deduplicator.storeResponseToMessage(reqMsg.getRequestId(), response);

		return response;
	}
}
