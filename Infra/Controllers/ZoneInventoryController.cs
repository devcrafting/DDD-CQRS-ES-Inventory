using Domain;
using Domain.ZoneInventories;

namespace Infra.Controllers
{
    public class ZoneInventoryController
    {
        private readonly IStoreZoneInventory _repository;

        public ZoneInventoryController(IStoreZoneInventory repository)
        {
            _repository = repository;
        }

        public void StartInventory(string zoneId, string inventoryId)
        {
            var zoneInventory = _repository.Get(zoneId); // get data required
            var events = zoneInventory.Start(zoneId, inventoryId); // pure logic
            _repository.Save(zoneId, events); // save data (incl. publish events)
        }
    }
}