package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.InventoryRepository;
import no.bouvet.solid.srpdip.OrderRepository;
import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.domain.OrderItemState;
import no.bouvet.solid.srpdip.domain.OrderState;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessageFactory;

public class CancelOrderOperation implements OrderOperation {

	public static Factory<CancelOrderOperation> factory = new Factory<>(CancelOrderOperation.class);

	private InventoryRepository inventoryRepository = InventoryRepository.factory.getInstance();
	private OrderRepository orderRepository = OrderRepository.factory.getInstance();
	private ResponseMessageFactory responseMessageFactory = ResponseMessageFactory.factory.getInstance();

	@Override
	public ResponseMessage execute(RequestMessage request) {
		Order orderToCancel = orderRepository.orders.get(request.getOrderId());

		switch (orderToCancel.getState()) {
		case SHIPPED:
			// msg cannot cancel shipped order
			return responseMessageFactory.createCancellationResponseMessage(request, orderToCancel,
					"Cannot cancel an order that has been shipped.");
		case CANCELLED:
			return responseMessageFactory.createCancellationResponseMessage(request, orderToCancel,
					"Order has already been cancelled.");
		default:
			orderToCancel.getOrderItems().forEach(item -> {
				if (item.getState() == OrderItemState.FILLED) {
					InventoryItem inventoryItem = inventoryRepository.inventory.get(item.getItemCode());
					inventoryItem.setQuantityOnHand(inventoryItem.getQuantityOnHand() + item.getQuantity());
				}
				item.setState(OrderItemState.CANCELLED);
			});

			orderToCancel.setState(OrderState.CANCELLED);

			// save inventory
			inventoryRepository.updateInventory();

			// save order
			orderRepository.updateOrders();

			return responseMessageFactory.createCancellationResponseMessage(request, orderToCancel,
					"Order has been cancelled and reserved inventory has been released.");
		}
	}

}
