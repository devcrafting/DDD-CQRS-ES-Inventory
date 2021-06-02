using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [Fact]
        public void ReturnNothing_WhenStartInventoryAnAlreadyStartedInventory()
        {
            var zoneId = Guid.NewGuid().ToString();
            var zoneInventory = new ZoneInventory(new ZoneInventoryStarted(zoneId));
            
            var events = zoneInventory.Start(zoneId);
            
            Check.That(events).IsEmpty();
        }

        [Fact]
        public void ReturnLocationScanned_WhenScanLocation()
        {
            var zoneId = Guid.NewGuid().ToString();
            var zoneInventory = new ZoneInventory(new ZoneInventoryStarted(zoneId));
            var locationId = Guid.NewGuid().ToString();
            
            var events = zoneInventory.ScanLocation(locationId);
            
            Check.That(events).ContainsExactly(new LocationScanned(zoneId, locationId));
        }

        [Fact]
        public void ThrowException_WhenScanLocationOnNotStartedInventory()
        {
            var zoneInventory = new ZoneInventory();
            var locationId = Guid.NewGuid().ToString();
            
            Check.ThatCode(() => zoneInventory.ScanLocation(locationId).ToList())
                .Throws<NotStartedInventory>();
        }
    }
}