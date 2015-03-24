package no.bouvet.solid.srpdip.domain.revenue;

import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.OrderItem;

public class FullQuantityPricingCalculation implements PricingCalculation {

	@Override
	public void calculate(OrderItem item, InventoryItem inventoryItem) {
		item.setWeightPerUnit(inventoryItem.getWeightPerUnit());
		item.setPricePerUnit(inventoryItem.getPricePerUnit());
		item.setRebatePerUnit(inventoryItem.getRebatePerUnit());

		item.setRebate(item.getRebatePerUnit() * item.getQuantity());
		item.setPrice(item.getPricePerUnit() * item.getQuantity() - item.getRebate());
		item.setVat(item.getPrice() * inventoryItem.getVatRate());
	}
}
