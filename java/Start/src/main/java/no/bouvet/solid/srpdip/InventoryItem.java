package no.bouvet.solid.srpdip;

public class InventoryItem {
	private String itemCode;
	private String pricingCalculation;
	private String unitName;
	private float weightPerUnit;
	private double pricePerUnit;
	private double rebatePerUnit;
	private double quantityOnHand;
	private double vatRate;

	public String getItemCode() {
		return itemCode;
	}

	public void setItemCode(String itemCode) {
		this.itemCode = itemCode;
	}

	public String getPricingCalculation() {
		return pricingCalculation;
	}

	public void setPricingCalculation(String pricingCalculation) {
		this.pricingCalculation = pricingCalculation;
	}

	public String getUnitName() {
		return unitName;
	}

	public void setUnitName(String unitName) {
		this.unitName = unitName;
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

	public double getQuantityOnHand() {
		return quantityOnHand;
	}

	public void setQuantityOnHand(double quantityOnHand) {
		this.quantityOnHand = quantityOnHand;
	}

	public double getVatRate() {
		return vatRate;
	}

	public void setVatRate(double vatRate) {
		this.vatRate = vatRate;
	}
}
