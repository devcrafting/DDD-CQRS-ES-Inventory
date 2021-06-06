namespace Domain.ZoneInventories.ZoneInventoriesLocations
{
    public interface IStoreZoneInventoriesLocation
    {
        void Save(ZoneInventoriesLocation zoneInventoriesLocation);
        ZoneInventoriesLocation Get(string locationScannedLocationId);
    }
}