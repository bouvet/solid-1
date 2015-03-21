namespace SRP_DI_Workshop.Domain.Revenue
{
    public class Buy2Get1FreePricingCalculation : IPricingCalculation
    {
        public const string Id = "BUY2GET1FREE";

        public void CalculatePrice(OrderItem item, InventoryItem inventoryItem)
        {
            item.WeightPerUnit = inventoryItem.WeightPerUnit;
            item.PricePerUnit = inventoryItem.PricePerUnit;
            item.RebatePerUnit = 0m;

            int multiplesOfThree = (int)item.Quantity / 3;
            decimal quantityTocharge = item.Quantity - multiplesOfThree;

            item.Weight = item.WeightPerUnit * (float)item.Quantity;
            item.Rebate = 0m;
            item.Price = item.PricePerUnit * quantityTocharge;
            item.Vat = item.Price * inventoryItem.VatRate;
        }
    }
}