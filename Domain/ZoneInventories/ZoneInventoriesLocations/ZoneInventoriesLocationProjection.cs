namespace Domain.ZoneInventories.ZoneInventoriesLocations
{
    public class ZoneInventoriesLocationProjection
    {
        private readonly IStoreZoneInventoriesLocation _repository;

        public ZoneInventoriesLocationProjection(IStoreZoneInventoriesLocation repository)
        {
            _repository = repository;
        }
        
        public void Handle(ZoneInventoryStarted zoneInventoryStarted)
        {
            foreach (var expectedItem in zoneInventoryStarted.ExpectedItems)
            {
                _repository.Save(new ZoneInventoriesLocation(expectedItem.LocationId, LocationStatus.Todo));
            }
        }
        
        public void Handle(LocationScanned locationScanned)
        {
            var location = _repository.Get(locationScanned.LocationId);
            location = new ZoneInventoriesLocation(location.ZoneId, LocationStatus.InProgress);
            _repository.Save(location);
        }
        
        public void Handle(ItemScanned itemScanned)
        {
            var location = _repository.Get(itemScanned.LocationId);
            location = new ZoneInventoriesLocation(location.ZoneId, LocationStatus.Done);
            _repository.Save(location);
        }
    }
}