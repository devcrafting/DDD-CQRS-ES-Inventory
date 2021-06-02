using System;
using System.Collections.Generic;

namespace Domain
{
    public class ZoneInventory
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> _evolveByEventType = new();
        
        private string _zoneId;
        private bool _started;
        private string _lastLocationScanned;

        private void Register<TEvent>(Action<TEvent> evolve) where TEvent : class
        {
            _evolveByEventType[typeof(TEvent)] = @event => evolve(@event as TEvent);
        }

        public ZoneInventory(params IDomainEvent[] history)
        {
            Register<ZoneInventoryStarted>(Evolve);
            Register<LocationScanned>(Evolve);
            foreach (var @event in history)
            {
                _evolveByEventType[@event.GetType()](@event);
            }
        }

        private void Evolve(ZoneInventoryStarted @event)
        {
            _zoneId = @event.ZoneId;
            _started = true;
        }

        private void Evolve(LocationScanned @event)
        {
            _lastLocationScanned = @event.LocationId;
        }

        public IEnumerable<IDomainEvent> Start(string zoneId)
        {
            if (!_started)
                yield return new ZoneInventoryStarted(zoneId);
        }

        public IEnumerable<IDomainEvent> ScanLocation(string locationId)
        {
            if (!_started)
                throw new NotStartedInventory();
            yield return new LocationScanned(_zoneId, locationId);
        }

        public IEnumerable<IDomainEvent> ScanItem(string itemId, int quantity)
        {
            if (_lastLocationScanned == null)
                throw new NoLocationScanned();
            yield return new ItemScanned(_zoneId, _lastLocationScanned, itemId, quantity);
        }
    }
}