using System;

namespace SRP_DI_Workshop.Domain.Revenue
{
    public class PriceCalculator
    {
        public void CalculatePrice(OrderItem item, InventoryItem inventoryItem)
        {
            IPricingCalculation pricingCalculation;

            switch (inventoryItem.PricingCalculation)
            {
                case FullQuantityPricingCalculation.Id:
                    pricingCalculation = new FullQuantityPricingCalculation();
                    break;
                case Buy2Get1FreePricingCalculation.Id:
                    pricingCalculation = new Buy2Get1FreePricingCalculation();
                    break;
                default:
                    throw new InvalidOperationException("Invalid pricing calculation type: " + inventoryItem.PricingCalculation);
            }

            pricingCalculation.CalculatePrice(item, inventoryItem);
        }
    }
}