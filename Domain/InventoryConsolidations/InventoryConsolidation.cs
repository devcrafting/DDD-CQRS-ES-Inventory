using System.Collections.Generic;
using Domain.ZoneInventories;

namespace Domain.InventoryConsolidations
{
    public class InventoryConsolidation
    {
        public IEnumerable<IDomainEvent> Analyze(ItemNotExpected itemNotExpected)
        {
            yield return new MissingItemDetected(itemNotExpected.ExpectedItem.ItemId, itemNotExpected.ExpectedItem.Quantity);
            yield return new ItemDiscovered(itemNotExpected.ItemId, itemNotExpected.Quantity);
        }

        public IEnumerable<IDomainEvent> Analyze(QuantityNotExpected quantityNotExpected)
        {
            var quantityDiff = quantityNotExpected.ExpectedItem.Quantity - quantityNotExpected.Quantity;
            if (quantityDiff > 0)
                yield return new MissingItemDetected(quantityNotExpected.ExpectedItem.ItemId, quantityDiff);
            else
                yield return new ItemDiscovered(quantityNotExpected.ExpectedItem.ItemId, -quantityDiff);
        }
    }
}