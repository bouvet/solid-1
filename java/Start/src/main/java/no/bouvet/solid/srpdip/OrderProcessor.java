package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.HashMap;
import java.util.Map;

import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.Order;
import no.bouvet.solid.srpdip.domain.OrderItem;
import no.bouvet.solid.srpdip.domain.OrderItemState;
import no.bouvet.solid.srpdip.domain.OrderState;
import no.bouvet.solid.srpdip.messageinterface.OrderCancellationResponse;
import no.bouvet.solid.srpdip.messageinterface.OrderQueryResponseMessage;
import no.bouvet.solid.srpdip.messageinterface.OrderSubmissionResponse;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;

import com.thoughtworks.xstream.XStream;

public class OrderProcessor {

	private static final String INVENTORY_FILE_NAME = "data/inventoryFile.xml";
	private static final String ORDER_FILE_NAME = "data/orderFile.xml";

	private Map<Long, ResponseMessage> processedMessages = new HashMap<>();
	private Map<String, InventoryItem> inventory;
	private Map<Long, Order> orders;

	private long lastOrderId = 0;

	public OrderProcessor() {
		readInventoryFromFile();
		readOrdersFromFile();

		lastOrderId = orders.values()
				.stream()
				.mapToLong(Order::getOrderId)
				.max()
				.orElse(0);
	}

	public ResponseMessage processMessage(RequestMessage reqMsg) {
		ResponseMessage responseToPreviousMessage = processedMessages.get(reqMsg.getRequestId());

		if (responseToPreviousMessage != null) {
			return responseToPreviousMessage;
		}

		ResponseMessage response = null;

		switch (reqMsg.getOperation()) {
		case SUBMIT_ORDER:
			response = createOrder(reqMsg);
			break;
		case CANCEL_ORDER:
			response = cancelOrder(reqMsg);
			break;
		case GET_ORDER_DETAILS:
			response = getOrder(reqMsg);
		}

		processedMessages.put(reqMsg.getRequestId(), response);

		return response;
	}

	private ResponseMessage getOrder(RequestMessage reqMsg) {
		Order orderToGet = orders.get(reqMsg.getOrderId());

		return createOrderQueryResponseMessage(reqMsg, orderToGet);
	}

	private static ResponseMessage createOrderQueryResponseMessage(RequestMessage reqMsg, Order orderToGet) {
		return new OrderQueryResponseMessage(reqMsg.getRequestId(), orderToGet);
	}

	private ResponseMessage cancelOrder(RequestMessage reqMsg) {
		Order orderToCancel = orders.get(reqMsg.getOrderId());

		switch (orderToCancel.getState()) {
		case SHIPPED:
			// msg cannot cancel shipped order
			return createCancellationResponseMessage(reqMsg, orderToCancel, "Cannot cancel an order that has been shipped.");
		case CANCELLED:
			return createCancellationResponseMessage(reqMsg, orderToCancel, "Order has already been cancelled.");
		default:
			orderToCancel.getOrderItems().forEach(item -> {
				if (item.getState() == OrderItemState.FILLED) {
					InventoryItem inventoryItem = inventory.get(item.getItemCode());
					inventoryItem.setQuantityOnHand(inventoryItem.getQuantityOnHand() + item.getQuantity());
				}
				item.setState(OrderItemState.CANCELLED);
			});

			orderToCancel.setState(OrderState.CANCELLED);

			// save inventory
			writeInventoryToFile();

			// save order
			writeOrdersToFile();

			return createCancellationResponseMessage(reqMsg, orderToCancel,
					"Order has been cancelled and reserved inventory has been released.");
		}
	}

	private static ResponseMessage createCancellationResponseMessage(RequestMessage reqMsg, Order orderToCancel, String message) {
		return new OrderCancellationResponse(reqMsg.getRequestId(), reqMsg.getOrderId(), orderToCancel.getState().toString(),
				message);
	}

	private ResponseMessage createOrder(RequestMessage reqMsg) {
		Order order = new Order(++lastOrderId);

		order.getOrderItems().addAll(reqMsg.getOrderItems());

		order.getOrderItems().forEach(item -> {
			InventoryItem inventoryItem = inventory.get(item.getItemCode());

			if (inventoryItem.getQuantityOnHand() <= item.getQuantity()) {
				inventoryItem.setQuantityOnHand(inventoryItem.getQuantityOnHand() - item.getQuantity());
				item.setWeight(item.getWeightPerUnit() * (float) item.getQuantity());

				calculatePrice(item, inventoryItem);

				item.setState(OrderItemState.FILLED);
			} else {
				item.setState(OrderItemState.NOT_ENOUGH_QUANTITY_ON_HAND);
			}
		});

		order.setState(order.getOrderItems().stream().allMatch(o -> o.getState() == OrderItemState.FILLED) 
				? OrderState.FILLED
				: OrderState.PROCESSING);

		orders.put(order.getOrderId(), order);

		// save inventory
		writeInventoryToFile();

		// save order
		writeOrdersToFile();

		return createOrderSubmissionResponseMessage(reqMsg, order);
	}

	private static ResponseMessage createOrderSubmissionResponseMessage(RequestMessage reqMsg, Order order) {
		return new OrderSubmissionResponse(reqMsg.getRequestId(), order.getOrderId(), order.getState().toString());
	}

	private static void calculatePrice(OrderItem item, InventoryItem inventoryItem) {
		if (inventoryItem.getPricingCalculation().equals("FULLQUANTITY")) {
			item.setWeightPerUnit(inventoryItem.getWeightPerUnit());
			item.setPricePerUnit(inventoryItem.getPricePerUnit());
			item.setRebatePerUnit(inventoryItem.getRebatePerUnit());

			item.setRebate(item.getRebatePerUnit() * item.getQuantity());
			item.setPrice(item.getPricePerUnit() * item.getQuantity() - item.getRebate());
			item.setVat(item.getPrice() * inventoryItem.getVatRate());
		} else if (inventoryItem.getPricingCalculation().equals("BUY2GET1FREE")) {
			item.setWeightPerUnit(inventoryItem.getWeightPerUnit());
			item.setPricePerUnit(inventoryItem.getPricePerUnit());
			item.setRebatePerUnit(0);

			int multiplesOfThree = (int) item.getQuantity() / 3;
			double quantityTocharge = item.getQuantity() - multiplesOfThree;

			item.setRebate(0);
			item.setPrice(item.getPricePerUnit() * quantityTocharge);
			item.setVat(item.getPrice() * inventoryItem.getVatRate());
		} else {
			throw new RuntimeException("Invalid pricing calculation type: " + inventoryItem.getPricingCalculation());
		}
	}

	@SuppressWarnings("unchecked")
	private void readInventoryFromFile() {
		inventory = (Map<String, InventoryItem>) new XStream().fromXML(new File(INVENTORY_FILE_NAME));
	}

	private void writeInventoryToFile() {
		try (OutputStream stream = new FileOutputStream(new File(INVENTORY_FILE_NAME))) {
			new XStream().toXML(inventory, stream);
		} catch (IOException e) {
			throw new RuntimeException("Failed to persist inventory: " + e.getMessage());
		}
	}

	@SuppressWarnings("unchecked")
	private void readOrdersFromFile() {
		orders = (Map<Long, Order>) new XStream().fromXML(new File(ORDER_FILE_NAME));
	}

	private void writeOrdersToFile() {
		try (OutputStream stream = new FileOutputStream(new File(ORDER_FILE_NAME))) {
			new XStream().toXML(orders, stream);
		} catch (IOException e) {
			throw new RuntimeException("Failed to persist orders: " + e.getMessage());
		}
	}
}
