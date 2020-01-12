public abstract class BuildingState
{
    public BuildingNearRoad Building { get; protected set; }
    public abstract void Initialize();
    public abstract void OnNewStorageAvaliable(StorageBuilding storageBuilding);
    public abstract void StartStopWork();
}
