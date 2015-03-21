namespace SRP_DI_Workshop.Domain.Revenue
{
    public interface IPricingCalculation
    {
        void CalculatePrice(OrderItem item, InventoryItem inventoryItem);
    }
}