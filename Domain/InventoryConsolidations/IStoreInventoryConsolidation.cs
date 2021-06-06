using System.Collections.Generic;

namespace Domain.InventoryConsolidations
{
    public interface IStoreInventoryConsolidation
    {
        InventoryConsolidation Get(string inventoryId);
        void Save(string inventoryId, IEnumerable<IDomainEvent> events);
    }
}