package no.bouvet.solid.srpdip;

public class OrderCancellationResponse extends ResponseMessage {
	private long orderId;
	public String orderState;
	public String message;

	public OrderCancellationResponse(long requestId, long orderId, String OrderState, String message) {
		super(requestId);
		this.orderId = orderId;
		this.orderState = orderState;
		this.message = message;
	}

	public long getOrderId() {
		return orderId;
	}

	public void setOrderId(long orderId) {
		this.orderId = orderId;
	}

	public String getOrderState() {
		return orderState;
	}

	public void setOrderState(String orderState) {
		this.orderState = orderState;
	}

	public String getMessage() {
		return message;
	}

	public void setMessage(String message) {
		this.message = message;
	}
}
