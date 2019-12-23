using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveBuildingNew : BuildingNearRoad
{
    [SerializeField] ActiveBuildingParams _abParams;
    public ActiveBuildingParams AbParams { get => _abParams; }
    public List<KeyValuePair<StorageBuilding, int>> ReachableStorages { get; private set; }

    public ActiveBuildingState CurrentState { get; private set; }
    public override void Initialize(Vector2Int tileID, Vector2Int roadConntectionPoint)
    {
        base.Initialize(tileID, roadConntectionPoint);
        CurrentState = new AB_State_CollectingMaterials(this);
        CurrentState.Initialize();
        Map.NewStorageBuildingPlaced += onNewStorageBuild;
    }
    public void ChangeState(ActiveBuildingState newState)
    {
        CurrentState = newState;
        newState.Initialize();
    }

    void onNewStorageBuild(StorageBuilding storageBuilding)
    {
        var path = Pathfinding.FindPath(_roadIDItConnects, storageBuilding.RoadIDItConnects);
        if (path != null)
        {
            KeyValuePair<StorageBuilding, int> kvp = new KeyValuePair<StorageBuilding, int>(storageBuilding, path.Count);
            if (ReachableStorages == null)
                ReachableStorages = new List<KeyValuePair<StorageBuilding, int>>();
            ReachableStorages.Add(kvp);
            ReachableStorages = ReachableStorages.OrderBy(d => d.Value).ToList();
            CurrentState.OnNewStorageAvaliable(storageBuilding);
        }
    }
    public void updateReachableStorages()
    {
        ReachableStorages = GameUtility.FindAllReachableStorages(RoadIDItConnects);
    }

}
