using System.Collections.Generic;
using Domain;

namespace Infra.Repositories
{
    public class StateRepository : IStoreZoneInventory
    {
        private Dictionary<string, ZoneInventory.ZoneInventoryState> _inMemoryDatabase = new();
        
        public ZoneInventory Get(string zoneId)
        {
            var zoneInventoryState = _inMemoryDatabase[zoneId];
            return new ZoneInventory(zoneInventoryState);
        }

        public void Save(string zoneId, IEnumerable<IDomainEvent> events)
        {
            var zoneInventoryState = _inMemoryDatabase[zoneId];
            zoneInventoryState.Evolve(events);
            // nothing to do since state in memory, but could be EF context.SaveChanges()
        }
    }
}