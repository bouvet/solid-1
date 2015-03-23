package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.domain.OrderItem;
import no.bouvet.solid.srpdip.domain.OrderItemState;
import no.bouvet.solid.srpdip.domain.OrderState;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;

public class CreateOrderOperation implements OrderOperation {

	@Override
	public ResponseMessage executeOperation(RequestMessage request) {
		try {
			Order order = new Order(++lastOrderId);

			order.getOrderItems().addAll(reqMsg.getOrderItems());

			for (OrderItem item : order.getOrderItems())
			{
				InventoryItem inventoryItem = inventory.get(item.getItemCode());

				if (inventoryItem.getQuantityOnHand() <= item.getQuantity())
				{
					inventoryItem.setQuantityOnHand(inventoryItem.getQuantityOnHand() - item.getQuantity());
					item.setWeight(item.getWeightPerUnit() * (float) item.getQuantity());

					calculatePrice(item, inventoryItem);

					item.setState(OrderItemState.FILLED);
				}
				else
				{
					item.setState(OrderItemState.NOT_ENOUGH_QUANTITY_ON_HAND);
				}
			}

			order.setState(
					order.getOrderItems().stream().allMatch(o -> o.getState() == OrderItemState.FILLED)
							? OrderState.FILLED
							: OrderState.PROCESSING);

			orders.put(order.getOrderId(), order);

			// save inventory
			writeInventoryToFile();

			// save order
			writeOrdersToFile();

			return createOrderSubmissionResponseMessage(reqMsg, order);
		} catch (Exception ex) {
			// Trace.WriteLine("Exception during operation SubmitOrder: " + ex,
			// "SubmitOrderError");
			return createErrorResponseMessage(reqMsg.getRequestId(), ex);
		}
	}

}
