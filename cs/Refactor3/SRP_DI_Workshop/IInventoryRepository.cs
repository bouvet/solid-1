using System.Collections.Generic;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop
{
    public interface IInventoryRepository
    {
        IDictionary<string, InventoryItem> Inventory { get; }
        void UpdateInventory();
    }
}