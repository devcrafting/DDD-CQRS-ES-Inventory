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

        public void StartInventory(string zoneId)
        {
            var zoneInventory = _repository.Get(zoneId);
            var events = zoneInventory.Start(zoneId);
            _repository.Save(zoneId, events);
        }
    }
}