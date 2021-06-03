using System;
using System.Collections.Generic;
using Domain;

namespace Infra
{
    public class PubSub
    {
        private readonly Dictionary<Type, List<Action<IDomainEvent>>> _subscribers = new();

        public void Publish(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                if (_subscribers.TryGetValue(@event.GetType(), out var subscribers))
                {
                    subscribers.ForEach(s => s(@event));
                }
            }
        }

        public void Register<TEvent>(Action<TEvent> subscriber) where TEvent : class
        {
            if (!_subscribers.TryGetValue(typeof(TEvent), out var subscribers))
            {
                subscribers = new List<Action<IDomainEvent>>();
                _subscribers[typeof(TEvent)] = subscribers;
            }
            subscribers.Add((e) => subscriber(e as TEvent));
        }
    }
}