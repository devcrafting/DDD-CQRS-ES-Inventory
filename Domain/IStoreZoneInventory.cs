using System.Collections.Generic;

namespace Domain
{
    public interface IStoreZoneInventory
    {
        ZoneInventory Get(string zoneId);
        void Save(string zoneId, IEnumerable<IDomainEvent> events);
    }
}