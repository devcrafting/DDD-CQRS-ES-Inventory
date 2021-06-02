using System;
using NFluent;
using Xunit;

namespace Domain.Tests
{
    public class ZoneInventory_Should
    {
        [Fact]
        public void ReturnZoneInventoryStarted_WhenStartInventory()
        {
            var zoneInventory = new ZoneInventory();
            var zoneId = Guid.NewGuid().ToString();
            
            var events = zoneInventory.Start(zoneId);
            
            Check.That(events).ContainsExactly(new ZoneInventoryStarted(zoneId));
        }
    }
}