package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.messageinterface.Operation;

import org.apache.log4j.Logger;

public class OperationFactory {

	public static Factory<OperationFactory> factory = new Factory<>(OperationFactory.class);

	private static final Logger LOG = Logger.getLogger(OperationFactory.class);

	public OrderOperation getInstance(Operation operation) {
		switch (operation) {
		case SUBMIT_ORDER:
			return CreateOrderOperation.factory.getInstance();
		case CANCEL_ORDER:
			return CancelOrderOperation.factory.getInstance();
		case GET_ORDER_DETAILS:
			return GetOrderOperation.factory.getInstance();
		default:
			LOG.warn("Received bad operation: " + operation);
			throw new RuntimeException("Received bad operation");
		}
	}
}
