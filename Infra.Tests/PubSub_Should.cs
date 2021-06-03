using Domain;
using NFluent;
using Xunit;

namespace Infra.Tests
{
    public class PubSub_Should
    {
        [Fact]
        public void CallSubscriber_WhenPublishEventWithRegisteredSubscriber()
        {
            var pubSub = new PubSub();
            FakeEvent receivedEvent = null;
            pubSub.Register<FakeEvent>(x => receivedEvent = x);
            
            pubSub.Publish(new IDomainEvent[]{ new FakeEvent(1) });

            Check.That(receivedEvent).Equals(new FakeEvent(1));
        }
        
        [Fact]
        public void IgnorePublishEventIfNoRegisteredSubscriber()
        {
            var pubSub = new PubSub();
            
            pubSub.Publish(new IDomainEvent[]{ new FakeEvent(1) });
            
            // Check that it still works
        }
        
        [Fact]
        public void CallSubscribers_WhenPublishEventWithSeveralRegisteredSubscribersForAnEventType()
        {
            var pubSub = new PubSub();
            FakeEvent receivedEvent = null;
            pubSub.Register<FakeEvent>(x => receivedEvent = x);
            FakeEvent otherReceivedEvent = null;
            pubSub.Register<FakeEvent>(x => otherReceivedEvent = x);
            
            pubSub.Publish(new IDomainEvent[]{ new FakeEvent(1) });

            Check.That(receivedEvent).Equals(new FakeEvent(1));
            Check.That(otherReceivedEvent).Equals(new FakeEvent(1));
        }
    }
    
    public record FakeEvent(int Id) : IDomainEvent;
}