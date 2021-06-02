namespace Domain
{
    public interface IDomainEvent
    {
    }
    
    public record ZoneInventoryStarted(string ZoneId) : IDomainEvent;
}