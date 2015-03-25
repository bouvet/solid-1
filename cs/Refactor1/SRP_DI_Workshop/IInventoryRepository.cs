using System.Collections.Generic;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop
{
    public interface IInventoryRepository
    {
        InventoryItem GetInventoryItem(string code);

        void UpdateInventory();
    }
}