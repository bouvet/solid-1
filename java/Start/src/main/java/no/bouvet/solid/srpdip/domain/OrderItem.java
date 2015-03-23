package no.bouvet.solid.srpdip.domain;


public class OrderItem {
	private long orderItemId;
	private String itemCode;
	private float weightPerUnit;
	private double pricePerUnit;
	private double rebatePerUnit;
	private double quantity;
	private double price;
	private double rebate;
	private double vat;
	private float weight;
	private OrderItemState state;

	public long getOrderItemId() {
		return orderItemId;
	}

	public void setOrderItemId(long orderItemId) {
		this.orderItemId = orderItemId;
	}

	public String getItemCode() {
		return itemCode;
	}

	public void setItemCode(String itemCode) {
		this.itemCode = itemCode;
	}

	public float getWeightPerUnit() {
		return weightPerUnit;
	}

	public void setWeightPerUnit(float weightPerUnit) {
		this.weightPerUnit = weightPerUnit;
	}

	public double getPricePerUnit() {
		return pricePerUnit;
	}

	public void setPricePerUnit(double pricePerUnit) {
		this.pricePerUnit = pricePerUnit;
	}

	public double getRebatePerUnit() {
		return rebatePerUnit;
	}

	public void setRebatePerUnit(double rebatePerUnit) {
		this.rebatePerUnit = rebatePerUnit;
	}

	public double getQuantity() {
		return quantity;
	}

	public void setQuantity(double quantity) {
		this.quantity = quantity;
	}

	public double getPrice() {
		return price;
	}

	public void setPrice(double price) {
		this.price = price;
	}

	public double getRebate() {
		return rebate;
	}

	public void setRebate(double rebate) {
		this.rebate = rebate;
	}

	public double getVat() {
		return vat;
	}

	public void setVat(double vat) {
		this.vat = vat;
	}

	public float getWeight() {
		return weight;
	}

	public void setWeight(float weight) {
		this.weight = weight;
	}

	public OrderItemState getState() {
		return state;
	}

	public void setState(OrderItemState state) {
		this.state = state;
	}
}
