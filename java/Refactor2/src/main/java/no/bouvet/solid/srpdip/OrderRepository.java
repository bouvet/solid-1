package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Map;

import no.bouvet.solid.srpdip.domain.Order;

import com.thoughtworks.xstream.XStream;

public class OrderRepository {

	public static Factory<OrderRepository> factory = new Factory<>(OrderRepository.class);

	private static final String ORDER_FILE_NAME = "data/orderFile.xml";

	private long lastOrderId = 0;

	private Map<Long, Order> orders;

	public OrderRepository() {
		readOrdersFromFile();
		
		lastOrderId = orders.values()
				.stream()
				.mapToLong(Order::getOrderId)
				.max()
				.orElse(0);
	}

	public Order createOrder() {
		return new Order(++lastOrderId);
	}
	
	public void addOrder(Order order) {
		orders.put(order.getOrderId(), order);
	}

	public Order getOrder(long orderId) {
		return orders.get(orderId);
	}

	public void updateOrders() {
		writeOrdersToFile();
	}

	@SuppressWarnings("unchecked")
	private void readOrdersFromFile() {
		orders = (Map<Long, Order>) new XStream().fromXML(new File(ORDER_FILE_NAME));
	}

	private void writeOrdersToFile() {
		try (OutputStream stream = new FileOutputStream(ORDER_FILE_NAME)) {
			new XStream().toXML(orders, stream);
		} catch (IOException e) {
			throw new RuntimeException("Failed to persist orders: " + e.getMessage());
		}
	}
}
