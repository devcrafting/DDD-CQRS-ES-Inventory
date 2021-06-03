# DDD-CQRS-ES training - Inventory domain

Demonstrate an example of implementation of :

- Event Sourced Aggregate with EventSourcingRepository in memory implementation
  - History of events as parameters in constructors => each Decide method (command) return events
  - State inner class to encapsulate Evolve methods and state derived from events
- Can also be state based Aggregate with StateRepository in memory implementation with few adaptations
  - Aggregate constructor taking Aggregate state as parameter instead of history of events
  - Use Evolve on state
- Fake web controller that
  - Gets Aggregate from database => not depending on state or event sourcing storage
  - Call method on Aggregate (command domain logic) => return events
  - Save events for the Aggregate (store then publish events)
  NB : differs a bit from training where we says that PubSub store then call subscribers/handlers, here PubSub only call handlers, it is called by Aggregate Repository
