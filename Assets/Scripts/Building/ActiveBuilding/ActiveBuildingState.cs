using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveBuildingState
{
    public ActiveBuildingNew building { get; protected set; }
    public abstract void Initialize();
    public abstract void OnNewStorageAvaliable(StorageBuilding storageBuilding);
    public abstract void StartStopWork();
}
