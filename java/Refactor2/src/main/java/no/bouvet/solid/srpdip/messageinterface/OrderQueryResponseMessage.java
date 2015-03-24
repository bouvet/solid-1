package no.bouvet.solid.srpdip.messageinterface;

import no.bouvet.solid.srpdip.domain.Order;

public class OrderQueryResponseMessage extends ResponseMessage {
	private Order order;

	public OrderQueryResponseMessage(long requestId, Order order) {
		super(requestId);
		this.order = order;
	}

	public Order getOrder() {
		return order;
	}

	public void setOrder(Order order) {
		this.order = order;
	}

}
