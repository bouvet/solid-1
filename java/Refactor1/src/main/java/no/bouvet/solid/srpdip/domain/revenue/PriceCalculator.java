package no.bouvet.solid.srpdip.domain.revenue;

import no.bouvet.solid.srpdip.domain.InventoryItem;
import no.bouvet.solid.srpdip.domain.OrderItem;

public class PriceCalculator
{
    public void calculate(OrderItem item, InventoryItem inventoryItem)
    {
        PricingCalculation pricingCalculation;

        switch (inventoryItem.getPricingCalculation())
        {
            case "FULLQUANTITY":
                pricingCalculation = new FullQuantityPricingCalculation();
                break;
            case "BUY2GET1FREE":
                pricingCalculation = new Buy2Get1FreePricingCalculation();
                break;
            default:
                throw new RuntimeException("Invalid pricing calculation type: " + inventoryItem.getPricingCalculation());
        }

        pricingCalculation.calculate(item, inventoryItem);
    }
}
