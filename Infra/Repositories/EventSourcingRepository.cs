using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Infra.Repositories
{
    public class EventSourcingRepository : IStoreZoneInventory
    {
        private readonly Dictionary<string, IEnumerable<IDomainEvent>> _inMemoryEventStore = new();
        
        public ZoneInventory Get(string zoneId)
        {
            return new ZoneInventory(_inMemoryEventStore[zoneId].ToArray());
        }

        public void Save(string zoneId, IEnumerable<IDomainEvent> events)
        {
            var existingEvents = _inMemoryEventStore[zoneId];
            _inMemoryEventStore[zoneId] = existingEvents.Concat(events);
        }
    }
}