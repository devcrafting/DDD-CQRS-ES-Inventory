using Domain.ZoneInventories;

namespace Domain.InventoryConsolidations
{
    public record MissingItemDetected(string ItemId, int Quantity) : IDomainEvent;

    public record ItemDiscovered(string ItemId, int Quantity) : IDomainEvent;
}