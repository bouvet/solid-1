namespace SRP_DI_Workshop.Domain.Revenue
{
    public interface IPriceCalculator
    {
        void CalculatePrice(OrderItem item, InventoryItem inventoryItem);
    }
}