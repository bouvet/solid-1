package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.InventoryRepository;
import no.bouvet.solid.srpdip.OrderRepository;
import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.domain.OrderItem;
import no.bouvet.solid.srpdip.domain.OrderItemState;
import no.bouvet.solid.srpdip.domain.OrderState;
import no.bouvet.solid.srpdip.domain.revenue.PriceCalculator;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessageFactory;

public class CreateOrderOperation implements OrderOperation {

	public static Factory<CreateOrderOperation> factory = new Factory<>(CreateOrderOperation.class);

	private InventoryRepository inventoryRepository = InventoryRepository.factory.getInstance();
	private OrderRepository orderRepository = OrderRepository.factory.getInstance();
	private ResponseMessageFactory responseMessageFactory = ResponseMessageFactory.factory.getInstance();
	private PriceCalculator priceCalculator = PriceCalculator.factory.getInstance();

	@Override
	public ResponseMessage execute(RequestMessage request) {
		try {
			Order order = orderRepository.createOrder();

			order.getOrderItems().addAll(request.getOrderItems());

			for (OrderItem item : order.getOrderItems())
			{
				InventoryItem inventoryItem = inventoryRepository.inventory.get(item.getItemCode());

				if (inventoryItem.getQuantityOnHand() <= item.getQuantity())
				{
					inventoryItem.setQuantityOnHand(inventoryItem.getQuantityOnHand() - item.getQuantity());
					item.setWeight(item.getWeightPerUnit() * (float) item.getQuantity());

					priceCalculator.calculate(item, inventoryItem);

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

			orderRepository.orders.put(order.getOrderId(), order);

			// save inventory
			inventoryRepository.updateInventory();

			// save order
			orderRepository.updateOrders();

			return responseMessageFactory.createOrderSubmissionResponseMessage(request, order);
		} catch (Exception ex) {
			// Trace.WriteLine("Exception during operation SubmitOrder: " + ex,
			// "SubmitOrderError");
			return responseMessageFactory.createErrorResponseMessage(request.getRequestId(), ex);
		}
	}
}
