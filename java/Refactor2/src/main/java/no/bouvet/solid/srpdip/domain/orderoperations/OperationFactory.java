package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.messageinterface.Operation;

public class OperationFactory {

	public static Factory<OperationFactory> factory = new Factory<>(OperationFactory.class);

	public OrderOperation getInstance(Operation operation) {
		switch (operation) {
		case SUBMIT_ORDER:
			return CreateOrderOperation.factory.getInstance();
		case CANCEL_ORDER:
			return CancelOrderOperation.factory.getInstance();
		case GET_ORDER_DETAILS:
			return GetOrderOperation.factory.getInstance();
		}
		return null;
	}
}
