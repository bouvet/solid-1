package no.bouvet.solid.srpdip.domain;

import java.util.ArrayList;
import java.util.List;

public class Order {
	private long orderId;
	private List<OrderItem> orderItems = new ArrayList<OrderItem>();
	private float totalWeight;
	private double rebateTotal;
	private double itemSubtotal;
	private double freightSubtotal;
	private double vatSubtotal;
	private double orderTotal;
	private OrderState state;

	public Order(long orderId) {
		this.orderId = orderId;
	}
	
	public long getOrderId() {
		return orderId;
	}

	public void setOrderId(long orderId) {
		this.orderId = orderId;
	}

	public List<OrderItem> getOrderItems() {
		return orderItems;
	}

	public void setOrderItems(List<OrderItem> orderItems) {
		this.orderItems = orderItems;
	}

	public float getTotalWeight() {
		return totalWeight;
	}

	public void setTotalWeight(float totalWeight) {
		this.totalWeight = totalWeight;
	}

	public double getRebateTotal() {
		return rebateTotal;
	}

	public void setRebateTotal(double rebateTotal) {
		this.rebateTotal = rebateTotal;
	}

	public double getItemSubtotal() {
		return itemSubtotal;
	}

	public void setItemSubtotal(double itemSubtotal) {
		this.itemSubtotal = itemSubtotal;
	}

	public double getFreightSubtotal() {
		return freightSubtotal;
	}

	public void setFreightSubtotal(double freightSubtotal) {
		this.freightSubtotal = freightSubtotal;
	}

	public double getVatSubtotal() {
		return vatSubtotal;
	}

	public void setVatSubtotal(double vatSubtotal) {
		this.vatSubtotal = vatSubtotal;
	}

	public double getOrderTotal() {
		return orderTotal;
	}

	public void setOrderTotal(double orderTotal) {
		this.orderTotal = orderTotal;
	}

	public OrderState getState() {
		return state;
	}

	public void setState(OrderState state) {
		this.state = state;
	}
}
