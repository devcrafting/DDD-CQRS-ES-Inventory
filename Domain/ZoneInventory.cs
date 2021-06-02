using System;
using System.Collections.Generic;

namespace Domain
{
    public class ZoneInventory
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> _evolveByEventType = new();
        
        private bool _started;

        private void Evolve(ZoneInventoryStarted @event)
        {
            _started = true;
        }

        private void Register<TEvent>(Action<TEvent> evolve) where TEvent : class
        {
            _evolveByEventType[typeof(TEvent)] = @event => evolve(@event as TEvent);
        }

        public ZoneInventory(params IDomainEvent[] history)
        {
            Register<ZoneInventoryStarted>(Evolve);
            foreach (var @event in history)
            {
                _evolveByEventType[@event.GetType()](@event);
            }
        }

        public IEnumerable<IDomainEvent> Start(string zoneId)
        {
            if (!_started)
                yield return new ZoneInventoryStarted(zoneId);
        }
    }
}