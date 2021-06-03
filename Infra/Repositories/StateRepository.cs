using System.Collections.Generic;
using Domain;
using Domain.ZoneInventories;

namespace Infra.Repositories
{
    public class StateRepository : IStoreZoneInventory
    {
        private Dictionary<string, ZoneInventory.ZoneInventoryState> _inMemoryDatabase = new();
        private PubSub _pubSub;

        public StateRepository(PubSub pubSub)
        {
            _pubSub = pubSub;
        }

        public ZoneInventory Get(string zoneId)
        {
            var zoneInventoryState = _inMemoryDatabase[zoneId];
            return new ZoneInventory(zoneInventoryState);
        }

        public void Save(string zoneId, IEnumerable<IDomainEvent> events)
        {
            // 1. Store **evolved** state
            var zoneInventoryState = _inMemoryDatabase[zoneId];
            zoneInventoryState.Evolve(events);
            // nothing to do since state in memory, but could be EF context.SaveChanges()
            
            // 2. Publish events
            _pubSub.Publish(events);
        }
    }
}