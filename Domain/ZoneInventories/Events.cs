namespace Domain.ZoneInventories
{
    public record ZoneInventoryStarted(string ZoneId, string InventoryId, params ExpectedItem[] ExpectedItems) : IDomainEvent;

    public record LocationScanned(string ZoneId, string LocationId) : IDomainEvent;

    public record ItemScanned(string ZoneId, string LocationId, string ItemId, int Quantity) : IDomainEvent;
    
    public record ItemNotExpected(string InventoryId, string ZoneId, ExpectedItem ExpectedItem, string ItemId,
        int Quantity) : IDomainEvent;
    
    public record QuantityNotExpected(string ZoneId, ExpectedItem ExpectedItem, int Quantity) : IDomainEvent;
}