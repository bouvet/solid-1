namespace SRP_DI_Workshop.Domain.Revenue
{
    public class PriceCalculator : IPriceCalculator
    {
        private readonly IPricingCalculationFactory _pricingCalculationFactory;

        public PriceCalculator(IPricingCalculationFactory pricingCalculationFactory)
        {
            _pricingCalculationFactory = pricingCalculationFactory;
        }

        public void CalculatePrice(OrderItem item, InventoryItem inventoryItem)
        {
            IPricingCalculation pricingCalculation = _pricingCalculationFactory.GetPricingCalculation(inventoryItem.PricingCalculation);
            pricingCalculation.CalculatePrice(item, inventoryItem);
        }
    }
}