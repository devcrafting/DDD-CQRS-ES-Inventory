namespace Domain.ZoneInventories.ZoneInventoriesLocations
{
    public record ZoneInventoriesLocation(string ZoneId, LocationStatus Status);

    public enum LocationStatus
    {
        Todo,
        InProgress,
        Done
    }
}