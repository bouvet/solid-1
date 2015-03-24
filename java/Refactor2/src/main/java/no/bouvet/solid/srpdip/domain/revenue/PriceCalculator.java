package no.bouvet.solid.srpdip.domain.revenue;

import no.bouvet.solid.srpdip.Factory;
import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.OrderItem;

public class PriceCalculator
{
	public static Factory<PriceCalculator> factory = new Factory<>(PriceCalculator.class);
	
    public void calculate(OrderItem item, InventoryItem inventoryItem)
    {
        PricingCalculation pricingCalculation;

        switch (inventoryItem.getPricingCalculation())
        {
            case "FULLQUANTITY":
                pricingCalculation = FullQuantityPricingCalculation.factory.getInstance();
                break;
            case "BUY2GET1FREE":
                pricingCalculation = Buy2Get1FreePricingCalculation.factory.getInstance();
                break;
            default:
                throw new RuntimeException("Invalid pricing calculation type: " + inventoryItem.getPricingCalculation());
        }

        pricingCalculation.calculate(item, inventoryItem);
    }
}
