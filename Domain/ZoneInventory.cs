using System;
using System.Collections.Generic;

namespace Domain
{
    public class ZoneInventory
    {
        public IEnumerable<IDomainEvent> Start(string zoneId)
        {
            yield return new ZoneInventoryStarted(zoneId);
        }
    }
}