package no.bouvet.solid.srpdip.domain.revenue;

import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.OrderItem;

public class Buy2Get1FreePricingCalculation implements PricingCalculation {

	@Override
	public void calculate(OrderItem item, InventoryItem inventoryItem) {
		item.setWeightPerUnit(inventoryItem.getWeightPerUnit());
		item.setPricePerUnit(inventoryItem.getPricePerUnit());
		item.setRebatePerUnit(0);

		int multiplesOfThree = (int) item.getQuantity() / 3;
		double quantityTocharge = item.getQuantity() - multiplesOfThree;

		item.setRebate(0);
		item.setPrice(item.getPricePerUnit() * quantityTocharge);
		item.setVat(item.getPrice() * inventoryItem.getVatRate());
	}
}
