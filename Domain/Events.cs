namespace Domain
{
    public interface IDomainEvent
    {
    }
    
    public record ZoneInventoryStarted(string ZoneId) : IDomainEvent;

    public record LocationScanned(string ZoneId, string LocationId) : IDomainEvent;

    public record ItemScanned(string ZoneId, string LocationId, string ItemId, int Quantity) : IDomainEvent;
}