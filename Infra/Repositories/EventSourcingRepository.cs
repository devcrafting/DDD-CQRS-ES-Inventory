using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.ZoneInventories;

namespace Infra.Repositories
{
    public class EventSourcingRepository : IStoreZoneInventory
    {
        private readonly PubSub _pubSub;
        private readonly Dictionary<string, IEnumerable<IDomainEvent>> _inMemoryEventStore = new();

        public EventSourcingRepository(PubSub pubSub)
        {
            _pubSub = pubSub;
        }
        
        public ZoneInventory Get(string zoneId)
        {
            return new ZoneInventory(_inMemoryEventStore[zoneId].ToArray());
        }

        public void Save(string zoneId, IEnumerable<IDomainEvent> events)
        {
            // 1. Store events
            var existingEvents = _inMemoryEventStore[zoneId];
            _inMemoryEventStore[zoneId] = existingEvents.Concat(events);
            
            // 2. Publish events
            _pubSub.Publish(events);
        }
    }
}