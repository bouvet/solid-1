package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.InventoryRepository;
import no.bouvet.solid.srpdip.OrderRepository;
import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.domain.OrderItemState;
import no.bouvet.solid.srpdip.domain.OrderState;
import no.bouvet.solid.srpdip.domain.revenue.PriceCalculator;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessageFactory;

public class CreateOrderOperation implements OrderOperation {

	private InventoryRepository inventoryRepository;
	private OrderRepository orderRepository;

	private ResponseMessageFactory responseMessageFactory = new ResponseMessageFactory();

	private PriceCalculator priceCalculator = new PriceCalculator();

	public CreateOrderOperation(InventoryRepository inventory, OrderRepository orders) {
		this.inventoryRepository = inventory;
		this.orderRepository = orders;
	}

	@Override
	public ResponseMessage execute(RequestMessage request) {
		Order order = orderRepository.createOrder();

		order.getOrderItems().addAll(request.getOrderItems());

		order.getOrderItems().forEach(item -> {
			InventoryItem inventoryItem = inventoryRepository.getInventoryItem(item.getItemCode());

			if (inventoryItem.getQuantityOnHand() >= item.getQuantity()) {
				inventoryItem.setQuantityOnHand(inventoryItem.getQuantityOnHand() - item.getQuantity());
				item.setWeight(item.getWeightPerUnit() * (float) item.getQuantity());

				priceCalculator.calculate(item, inventoryItem);

				item.setState(OrderItemState.FILLED);
			} else {
				item.setState(OrderItemState.NOT_ENOUGH_QUANTITY_ON_HAND);
			}
		});

		order.setState(order.getOrderItems().stream().allMatch(o -> o.getState() == OrderItemState.FILLED) ? OrderState.FILLED
				: OrderState.PROCESSING);

		orderRepository.addOrder(order);

		// save inventory
		inventoryRepository.updateInventory();

		// save order
		orderRepository.updateOrders();

		return responseMessageFactory.createOrderSubmissionResponseMessage(request, order);
	}

}
