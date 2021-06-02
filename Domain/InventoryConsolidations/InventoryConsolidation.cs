using System;
using System.Collections.Generic;
using Domain.ZoneInventories;

namespace Domain.InventoryConsolidations
{
    public class InventoryConsolidation
    {
        private readonly InventoryConsolidationState _state;

        public InventoryConsolidation(params IDomainEvent[] history)
        {
            _state = new InventoryConsolidationState(history);
        }

        public IEnumerable<IDomainEvent> Analyze(ItemNotExpected itemNotExpected)
        {
            if (_state.MissingItems.TryGetValue(itemNotExpected.ItemId, out var missingItems))
                foreach (var missingItem in missingItems)
                {
                    yield return new ItemFoundInAnotherLocation(itemNotExpected.ItemId, itemNotExpected.Quantity, missingItem.locationId, itemNotExpected.ExpectedItem.LocationId);
                }
            else
            {
                yield return new MissingItemDetected(itemNotExpected.ExpectedItem.LocationId, itemNotExpected.ExpectedItem.ItemId, itemNotExpected.ExpectedItem.Quantity);
                yield return new ItemDiscovered(itemNotExpected.ExpectedItem.LocationId, itemNotExpected.ItemId, itemNotExpected.Quantity);
            }
        }

        public IEnumerable<IDomainEvent> Analyze(QuantityNotExpected quantityNotExpected)
        {
            var quantityDiff = quantityNotExpected.ExpectedItem.Quantity - quantityNotExpected.Quantity;
            if (quantityDiff > 0)
                yield return new MissingItemDetected(quantityNotExpected.ExpectedItem.LocationId, quantityNotExpected.ExpectedItem.ItemId, quantityDiff);
            else
                yield return new ItemDiscovered(quantityNotExpected.ExpectedItem.LocationId, quantityNotExpected.ExpectedItem.ItemId, -quantityDiff);
        }
        
        
        public class InventoryConsolidationState
        {
            private readonly Dictionary<Type, Action<IDomainEvent>> _evolveByEventType = new();

            public Dictionary<string, List<(string locationId, int quantity)>> MissingItems { get; } = new (); 

            private void Register<TEvent>(Action<TEvent> evolve) where TEvent : class
            {
                _evolveByEventType[typeof(TEvent)] = @event => evolve(@event as TEvent);
            }

            public InventoryConsolidationState(IEnumerable<IDomainEvent> history)
            {
                Register<MissingItemDetected>(Evolve);
                Register<ItemDiscovered>(Evolve);
                Evolve(history);
            }

            private void Evolve(IEnumerable<IDomainEvent> history)
            {
                foreach (var @event in history)
                {
                    _evolveByEventType[@event.GetType()](@event);
                }
            }

            private void Evolve(MissingItemDetected @event)
            {
                if (!MissingItems.TryGetValue(@event.ItemId, out var missingItems))
                {
                    missingItems = new List<(string, int)>();
                    MissingItems[@event.ItemId] = missingItems;
                }
                missingItems.Add((@event.LocationId, @event.Quantity));
            }

            private void Evolve(ItemDiscovered @event)
            {
            }
        }
    }
}