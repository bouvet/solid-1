namespace SRP_DI_Workshop.Domain.Revenue
{
    public class FullQuantityPricingCalculation : IPricingCalculation
    {
        public const string Id = "FULLQUANTITY";

        public void CalculatePrice(OrderItem item, InventoryItem inventoryItem)
        {
            item.WeightPerUnit = inventoryItem.WeightPerUnit;
            item.PricePerUnit = inventoryItem.PricePerUnit;
            item.RebatePerUnit = inventoryItem.RebatePerUnit;

            item.Weight = item.WeightPerUnit * (float)item.Quantity;
            item.Rebate = item.RebatePerUnit * item.Quantity;
            item.Price = item.PricePerUnit * item.Quantity - item.Rebate;
            item.Vat = item.Price * inventoryItem.VatRate;
        }
    }
}