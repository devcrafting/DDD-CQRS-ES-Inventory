using Domain.ZoneInventories;

namespace Domain.InventoryConsolidations
{
    public record MissingItemDetected(string LocationId, string ItemId, int Quantity) : IDomainEvent;

    public record ItemDiscovered(string LocationId, string ItemId, int Quantity) : IDomainEvent;

    public record ItemFoundInAnotherLocation(
        string ItemId,
        int Quantity,
        string ExpectedLocationId,
        string FoundLocationId) : IDomainEvent;
}