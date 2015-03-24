package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.util.Map;

import no.bouvet.solid.srpdip.domain.Order;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.thoughtworks.xstream.XStream;

public class OrderRepository {

	private static final Logger LOG = LoggerFactory.getLogger(OrderRepository.class);

	private static final String ORDER_FILE_NAME = "orderFile.xml";

	private long lastOrderId = 0;

	public Map<Long, Order> orders;

	public OrderRepository() {
		readOrdersFromFile();
		lastOrderId = orders.values().stream().mapToLong(Order::getOrderId).max().orElse(0);
	}

	public Order createOrder() {
		return new Order(++lastOrderId);
	}

	public void updateOrders() {
		writeOrdersToFile();
	}

	@SuppressWarnings("unchecked")
	private void readOrdersFromFile() {
		try {
			orders = (Map<Long, Order>) new XStream().fromXML(getClass().getClassLoader().getResourceAsStream(ORDER_FILE_NAME));
		} catch (Exception ex) {
			LOG.error("Exception while trying to read orders from file: ", ex);
			// throw ex;
		}
	}

	private void writeOrdersToFile() {
		try {
			new XStream().toXML(orders, new FileOutputStream(new File(ORDER_FILE_NAME)));
		} catch (Exception ex) {
			LOG.error("Exception while trying to write orders to file: ", ex);
			// throw ex;
		}
	}

}
