using System.Collections.Generic;

namespace Domain.ZoneInventories
{
    public interface IStoreZoneInventory
    {
        ZoneInventory Get(string zoneId);
        void Save(string zoneId, IEnumerable<IDomainEvent> events);
    }
}