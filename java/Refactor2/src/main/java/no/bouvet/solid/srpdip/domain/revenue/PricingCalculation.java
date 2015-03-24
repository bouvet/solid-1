package no.bouvet.solid.srpdip.domain.revenue;

import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.OrderItem;

public interface PricingCalculation {
	void calculate(OrderItem item, InventoryItem inventoryItem);
}
