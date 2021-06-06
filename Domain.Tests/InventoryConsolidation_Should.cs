using System;
using Domain.InventoryConsolidations;
using Domain.ZoneInventories;
using NFluent;
using Xunit;

namespace Domain.Tests
{
    public class InventoryConsolidation_Should
    {
        [Fact]
        public void ReturnMissingItemFoundAndItemDiscovered_WhenReceivingItemNotExpected()
        {
            var inventoryConsolidation = new InventoryConsolidation();
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var expectedItemId = Guid.NewGuid().ToString();
            var expectedItem = new ExpectedItem(locationId, expectedItemId, 3);
            
            var foundItemId = Guid.NewGuid().ToString();
            var foundQuantity = 4;
            var itemNotExpected = new ItemNotExpected(inventoryId, zoneId, expectedItem, foundItemId, foundQuantity);

            var events = inventoryConsolidation.Analyze(itemNotExpected);

            Check.That(events).ContainsExactly(
                new MissingItemDetected(locationId, expectedItem.ItemId, expectedItem.Quantity),
                new ItemDiscovered(locationId, foundItemId, foundQuantity));
        }
        
        [Fact]
        public void ReturnMissingItemFound_WhenReceivingQuantityNotExpectedWithLowerQuantity()
        {
            var inventoryConsolidation = new InventoryConsolidation();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var expectedItemId = Guid.NewGuid().ToString();
            var expectedItem = new ExpectedItem(locationId, expectedItemId, 3);
            
            var foundQuantity = 2;
            var quantityNotExpected = new QuantityNotExpected(zoneId, expectedItem, foundQuantity);

            var events = inventoryConsolidation.Analyze(quantityNotExpected);

            Check.That(events).ContainsExactly(
                new MissingItemDetected(locationId, expectedItem.ItemId, expectedItem.Quantity - foundQuantity));
        }
        
        [Fact]
        public void ReturnItemDiscovered_WhenReceivingQuantityNotExpectedWithHigherQuantity()
        {
            var inventoryConsolidation = new InventoryConsolidation();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var expectedItemId = Guid.NewGuid().ToString();
            var expectedItem = new ExpectedItem(locationId, expectedItemId, 3);
            
            var foundQuantity = 4;
            var quantityNotExpected = new QuantityNotExpected(zoneId, expectedItem, foundQuantity);

            var events = inventoryConsolidation.Analyze(quantityNotExpected);

            Check.That(events).ContainsExactly(
                new ItemDiscovered(locationId, expectedItem.ItemId, foundQuantity - expectedItem.Quantity));
        }
        
        [Fact]
        public void ReturnItemFoundInAnotherLocation_WhenReceivingItemNotExpectedFoundWithMissingSameItemInAnotherLocation()
        {
            var inventoryId = Guid.NewGuid().ToString();
            var zoneId = Guid.NewGuid().ToString();
            var locationId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();
            var inventoryConsolidation = new InventoryConsolidation(
                new MissingItemDetected(locationId, itemId, 3));
            var foundQuantity = 2;
            var anotherLocationId = Guid.NewGuid().ToString();
            var expectedItem = new ExpectedItem(anotherLocationId, itemId, 3);
            var itemNotExpected = new ItemNotExpected(inventoryId, zoneId, expectedItem, itemId, foundQuantity);

            var events = inventoryConsolidation.Analyze(itemNotExpected);

            Check.That(events).ContainsExactly(
                new ItemFoundInAnotherLocation(expectedItem.ItemId, foundQuantity, locationId, anotherLocationId));
        }
    }
}