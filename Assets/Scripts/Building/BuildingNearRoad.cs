using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingNearRoad : Building
{
    public Vector2Int RoadIDItConnects { get; protected set; }
    protected Road roadItConnects;
    public List<KeyValuePair<StorageBuilding, int>> ReachableStorages { get; protected set; }
    public virtual void Initialize(Vector2Int tileID, Vector2Int roadIDItConnects)
    {
        base.Initialize(tileID);
        RoadIDItConnects = roadIDItConnects;
        roadItConnects = GameUtility.GetRoadByID(roadIDItConnects);
        ReachableStorages = GameUtility.FindAllReachableStorages(RoadIDItConnects);
    }
    protected abstract void onNewStorageBuild(StorageBuilding storageBuilding);
}
