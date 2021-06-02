using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.ZoneInventories
{
    public class ZoneInventory
    {
        private readonly ZoneInventoryState _zoneInventoryState;

        // Constructor for Event Sourced aggregate 
        public ZoneInventory(params IDomainEvent[] history)
        {
            _zoneInventoryState = new ZoneInventoryState(history);
        }

        // Constructor for State based aggregate storage
        public ZoneInventory(ZoneInventoryState zoneInventoryState)
        {
            _zoneInventoryState = zoneInventoryState;
        }

        public IEnumerable<IDomainEvent> Start(string zoneId)
        {
            if (!_zoneInventoryState.Started)
                yield return new ZoneInventoryStarted(zoneId);
        }

        public IEnumerable<IDomainEvent> ScanLocation(string locationId)
        {
            if (!_zoneInventoryState.Started)
                throw new NotStartedInventory();
            yield return new LocationScanned(_zoneInventoryState.ZoneId, locationId);
        }

        public IEnumerable<IDomainEvent> ScanItem(string itemId, int quantity)
        {
            if (_zoneInventoryState.LastLocationScanned == null)
                throw new NoLocationScanned();
            
            var expectedItem = _zoneInventoryState.ExpectedItems.Single(x => x.LocationId == _zoneInventoryState.LastLocationScanned);
            if (expectedItem.ItemId != itemId)
                yield return new ItemNotExpected(_zoneInventoryState.ZoneId, expectedItem, itemId, quantity);
            else if (expectedItem.Quantity != quantity)
                yield return new QuantityNotExpected(_zoneInventoryState.ZoneId, expectedItem, quantity);
            else
                yield return new ItemScanned(_zoneInventoryState.ZoneId, _zoneInventoryState.LastLocationScanned, itemId, quantity);
        }

        public class ZoneInventoryState
        {
            public string ZoneId { get; private set; }
            public string LastLocationScanned { get; private set; }
            public IEnumerable<ExpectedItem> ExpectedItems { get; private set; }

            private readonly Dictionary<Type, Action<IDomainEvent>> _evolveByEventType = new();

            public bool Started { get; private set; }

            private void Register<TEvent>(Action<TEvent> evolve) where TEvent : class
            {
                _evolveByEventType[typeof(TEvent)] = @event => evolve(@event as TEvent);
            }

            public ZoneInventoryState(IEnumerable<IDomainEvent> history)
            {
                Register<ZoneInventoryStarted>(Evolve);
                Register<LocationScanned>(Evolve);
                Evolve(history);
            }

            // public only for state based aggregate storage OR need to evolve event sourced aggregate after commands (keep in memory for example)
            public void Evolve(IEnumerable<IDomainEvent> history)
            {
                foreach (var @event in history)
                {
                    _evolveByEventType[@event.GetType()](@event);
                }
            }

            private void Evolve(ZoneInventoryStarted @event)
            {
                ZoneId = @event.ZoneId;
                Started = true;
                ExpectedItems = @event.ExpectedItems ?? Array.Empty<ExpectedItem>();
            }

            private void Evolve(LocationScanned @event)
            {
                LastLocationScanned = @event.LocationId;
            }
        }
    }
}