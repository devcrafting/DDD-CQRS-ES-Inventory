using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ZoneInventories;
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
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            
            var events = zoneInventory.Start(zoneId, inventoryId);
            
            Check.That(events).ContainsExactly(new ZoneInventoryStarted(zoneId, inventoryId));
        }
        
        [Fact]
        public void ReturnNothing_WhenStartInventoryAnAlreadyStartedInventory()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var zoneInventory = new ZoneInventory(new ZoneInventoryStarted(zoneId, inventoryId));
            
            var events = zoneInventory.Start(zoneId, inventoryId);
            
            Check.That(events).IsEmpty();
        }

        [Fact]
        public void ReturnLocationScanned_WhenScanLocation()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var zoneInventory = new ZoneInventory(new ZoneInventoryStarted(zoneId, inventoryId));
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
        
        [Fact]
        public void ReturnItemScanned_WhenScanItem()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();
            var quantity = 3;
            var zoneInventory = new ZoneInventory(
                new ZoneInventoryStarted(zoneId, inventoryId,
                    new ExpectedItem(locationId, itemId, quantity)),
                new LocationScanned(zoneId, locationId));
            
            var events = zoneInventory.ScanItem(itemId, quantity);
            
            Check.That(events).ContainsExactly(
                new ItemScanned(zoneId, locationId, itemId, quantity));
        }
        
        [Fact]
        public void ThrowNoLocationScanned_WhenScanItemWithoutLastLocationScanned()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var zoneInventory = new ZoneInventory(
                new ZoneInventoryStarted(zoneId, inventoryId));
            var itemId = Guid.NewGuid().ToString();
            var quantity = 3;
            
            Check.ThatCode(() => zoneInventory.ScanItem(itemId, quantity).ToList())
                .Throws<NoLocationScanned>();
        }
        
        [Fact]
        public void ReturnItemNotExpected_WhenScanItem()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var expectedItemId = Guid.NewGuid().ToString();
            var expectedItem = new ExpectedItem(locationId, expectedItemId, 3);
            var zoneInventory = new ZoneInventory(
                new ZoneInventoryStarted(zoneId, inventoryId, expectedItem),
                new LocationScanned(zoneId, locationId));
            var itemId = Guid.NewGuid().ToString();
            var quantity = 3;
            
            var events = zoneInventory.ScanItem(itemId, quantity);
            
            Check.That(events).ContainsExactly(
                new ItemNotExpected(inventoryId, zoneId, expectedItem, itemId, quantity));
        }
        
        [Fact]
        public void ReturnQuantityNotExpected_WhenScanItem()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();
            var expectedQuantity = 3;
            var expectedItem = new ExpectedItem(locationId, itemId, expectedQuantity);
            var zoneInventory = new ZoneInventory(
                new ZoneInventoryStarted(zoneId, inventoryId,
                    expectedItem),
                new LocationScanned(zoneId, locationId));
            var notExpectedQuantity = 2;
            
            var events = zoneInventory.ScanItem(itemId, notExpectedQuantity);
            
            Check.That(events).ContainsExactly(
                new QuantityNotExpected(zoneId, expectedItem, notExpectedQuantity));
        }
    }
}