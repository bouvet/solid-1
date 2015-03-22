package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.util.HashMap;
import java.util.Map;

import org.apache.log4j.Logger;

import com.thoughtworks.xstream.XStream;

public class OrderProcessor {

	private static final String INVENTORY_FILE_NAME = "inventoryFile.xml";
	private static final String ORDER_FILE_NAME = "orderFile.xml";

	private static final Logger LOG = Logger.getLogger(OrderProcessor.class);

	private Map<Long, ResponseMessage> processedMessages = new HashMap<>();
	private Map<String, InventoryItem> inventory;
	private Map<Long, Order> orders;

	private long _lastOrderId = 0;

	public OrderProcessor()
	{
		readInventoryFromFile();
		readOrdersFromFile();
		_lastOrderId = orders.values().stream()
				.mapToLong(Order::getOrderId)
				.max()
				.orElse(0);
	}

	public ResponseMessage processMessage(RequestMessage reqMsg) {
		ResponseMessage responseToPreviousMessage = processedMessages.get(reqMsg.getRequestId());

		if (responseToPreviousMessage != null) {
			return responseToPreviousMessage;
		}

		ResponseMessage response;

		switch (reqMsg.getOperation())
		{
		case SUBMIT_ORDER:
			response = createOrder(reqMsg);
			break;
		case CANCEL_ORDER:
			response = cancelOrder(reqMsg);
			break;
		case GET_ORDER_DETAILS:
			response = getOrder(reqMsg);
			break;
		default:
			LOG.warn("Received bad message: " + reqMsg.getRequestId());
			throw new RuntimeException("Received bad message");
		}

		processedMessages.put(reqMsg.getRequestId(), response);

		return response;
	}

	private ResponseMessage getOrder(RequestMessage reqMsg)
	{
		try {
			Order orderToGet = orders.get(reqMsg.getOrderId());

			return createOrderQueryResponseMessage(reqMsg, orderToGet);
		} catch (Exception ex) {
			// Trace.WriteLine("Exception during operation GetOrderDetails: " +
			// ex, "GetOrderDetailsError");
			return createErrorResponseMessage(reqMsg.getRequestId(), ex);
		}
	}

	private static ResponseMessage createOrderQueryResponseMessage(RequestMessage reqMsg, Order orderToGet)
	{
		return new OrderQueryResponseMessage(reqMsg.getRequestId(), orderToGet);
	}

	private ResponseMessage cancelOrder(RequestMessage reqMsg)
	{
		try {
			Order orderToCancel = orders.get(reqMsg.getOrderId());

			switch (orderToCancel.getState())
			{
			case SHIPPED:
				// msg cannot cancel shipped order
				return createCancellationResponseMessage(reqMsg, orderToCancel,
						"Cannot cancel an order that has been shipped.");
			case CANCELLED:
				return createCancellationResponseMessage(reqMsg, orderToCancel,
						"Order has already been cancelled.");
			default:
				for (OrderItem item : orderToCancel.getOrderItems())
				{
					InventoryItem inventoryItem = inventory.get(item.getItemCode());

					if (item.getState() == OrderItemState.FILLED)
						inventoryItem.setQuantityOnHand(
								inventoryItem.getQuantityOnHand() + item.getQuantity());

					item.setState(OrderItemState.CANCELLED);
				}

				orderToCancel.setState(OrderState.CANCELLED);

				// save inventory
				writeInventoryToFile();

				// save order
				writeOrdersToFile();

				return createCancellationResponseMessage(reqMsg, orderToCancel,
						"Order has been cancelled and reserved inventory has been released.");
			}
		} catch (Exception ex) {
			// Trace.WriteLine("Exception during operation CancelOrder: " + ex,
			// "CancelOrderError");
			return createErrorResponseMessage(reqMsg.getRequestId(), ex);
		}
	}

	private static ResponseMessage createCancellationResponseMessage(RequestMessage reqMsg, Order orderToCancel, String message)
	{
		return new OrderCancellationResponse(
				reqMsg.getRequestId(),
				reqMsg.getOrderId(),
				orderToCancel.getState().toString(),
				message);
	}

	private ResponseMessage createOrder(RequestMessage reqMsg)
	{
		try {
			Order order = new Order(++_lastOrderId);

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

	private static ResponseMessage createErrorResponseMessage(long requestId, Exception ex)
	{
		return new ResponseMessage(requestId);
		// {
		// StatusCode = HttpStatusCode.InternalServerError,
		// ResponseBody = ex.ToString()
		// };
	}

	private static ResponseMessage createOrderSubmissionResponseMessage(RequestMessage reqMsg, Order order)
	{
		return new OrderSubmissionResponse(
				reqMsg.getRequestId(),
				order.getOrderId(),
				order.getState().toString());
	}

	private static void calculatePrice(OrderItem item, InventoryItem inventoryItem)
	{
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
	private void readInventoryFromFile()
	{
		try {
			inventory = (Map<String, InventoryItem>) new XStream().fromXML(getClass().getClassLoader().getResourceAsStream(
					INVENTORY_FILE_NAME));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to read inventory from file: "
			// + ex, "InventoryError");
			throw ex;
		}
	}

	private void writeInventoryToFile() throws Exception
	{
		try {
			new XStream().toXML(inventory, new FileOutputStream(new File(INVENTORY_FILE_NAME)));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to write inventory to file: "
			// + ex, "InventoryError");
			throw ex;
		}
	}

	@SuppressWarnings("unchecked")
	private void readOrdersFromFile()
	{
		try {
			orders = (Map<Long, Order>) new XStream().fromXML(getClass().getClassLoader().getResourceAsStream(ORDER_FILE_NAME));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to read orders from file: "
			// + ex, "OrderError");
			throw ex;
		}
	}

	private void writeOrdersToFile() throws Exception
	{
		try {
			new XStream().toXML(orders, new FileOutputStream(new File(ORDER_FILE_NAME)));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to write orders to file: "
			// + ex, "OrdersError");
			throw ex;
		}
	}
}
