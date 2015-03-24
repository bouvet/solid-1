package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.util.Map;

import com.thoughtworks.xstream.XStream;

import no.bouvet.solid.srpdip.domain.Order;

public class OrderRepository {

	public static Factory<OrderRepository> factory = new Factory<>(OrderRepository.class);

	private static final String ORDER_FILE_NAME = "orderFile.xml";
	
	private long lastOrderId = 0;
	
	public Map<Long, Order> orders;

	public OrderRepository()
	{
		readOrdersFromFile();
		lastOrderId = orders.values().stream()
				.mapToLong(Order::getOrderId)
				.max()
				.orElse(0);
	}
	
	public Order createOrder() {
		return new Order(++lastOrderId);
	}

    public void updateOrders()
    {
        writeOrdersToFile();
    }

	@SuppressWarnings("unchecked")
	private void readOrdersFromFile()
	{
		try {
			orders = (Map<Long, Order>) new XStream().fromXML(getClass().getClassLoader().getResourceAsStream(ORDER_FILE_NAME));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to read orders from file: "
			// + ex, "OrderError");
			//throw ex;
		}
	}

	private void writeOrdersToFile()
	{
		try {
			new XStream().toXML(orders, new FileOutputStream(new File(ORDER_FILE_NAME)));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to write orders to file: "
			// + ex, "OrdersError");
			//throw ex;
		}
	}

}
