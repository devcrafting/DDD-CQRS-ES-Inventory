namespace Domain
{
    public interface IDomainEvent
    {
    }
    
    public record ZoneInventoryStarted(string ZoneId, params ExpectedItem[] ExpectedItems) : IDomainEvent;

    public record LocationScanned(string ZoneId, string LocationId) : IDomainEvent;

    public record ItemScanned(string ZoneId, string LocationId, string ItemId, int Quantity) : IDomainEvent;
    
    public record ItemNotExpected(string ZoneId, ExpectedItem ExpectedItem, string ItemId, int Quantity) : IDomainEvent;
    
    public record QuantityNotExpected(string ZoneId, ExpectedItem ExpectedItem, int Quantity) : IDomainEvent;
}