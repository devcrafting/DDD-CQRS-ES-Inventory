using Domain.ZoneInventories;

namespace Domain.InventoryConsolidations
{
    public class ConsolidationProcess
    {
        private readonly IStoreInventoryConsolidation _inventoryConsolidationRepository;

        public ConsolidationProcess(IStoreInventoryConsolidation inventoryConsolidationRepository)
        {
            _inventoryConsolidationRepository = inventoryConsolidationRepository;
        }
        
        public void Handle(ItemNotExpected itemNotExpected)
        {
            var inventoryConsolidation = _inventoryConsolidationRepository.Get(itemNotExpected.InventoryId);
            var events = inventoryConsolidation.Analyze(itemNotExpected);
            _inventoryConsolidationRepository.Save(itemNotExpected.InventoryId, events);
        }
    }
}