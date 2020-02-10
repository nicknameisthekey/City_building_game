using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveBuilding : BuildingNearRoad
{
    #region BuildingParams
    [SerializeField] ActiveBuildingParams _abParams;
    public ActiveBuildingParams AbParams { get => _abParams; }
    #endregion

    public override void Initialize(Vector2Int tileID, Vector2Int roadConnectionPoint)
    {
        base.Initialize(tileID, roadConnectionPoint);

        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[ActiveBuilding] {BuildingName} инициализирован в {tileID}, прилегающая дорога " +
            $"{roadConnectionPoint}, нашел [{ReachableStorages.Count}] достижимых складов. [{Time.deltaTime}]", gameObject);
        CurrentState = new State_Construction(this);
        CurrentState.Initialize();
        Map.NewStorageBuildingPlaced += onNewStorageBuild;
    }
    public void StartStopWork()
    {
        CurrentState.StartStopWork();
    }
    public override void changeState(BuildingState newState)
    {
        CurrentState = newState;
        newState.Initialize();
        stateChangedInvoke(newState);
    }

    protected override void onNewStorageBuild(StorageBuilding storageBuilding)
    {
        var path = Pathfinding.FindPath(RoadIDItConnects, storageBuilding.RoadIDItConnects);
        if (path != null)
        {
            KeyValuePair<StorageBuilding, int> kvp = new KeyValuePair<StorageBuilding, int>(storageBuilding, path.Count);
            if (ReachableStorages == null)
                ReachableStorages = new List<KeyValuePair<StorageBuilding, int>>();
            ReachableStorages.Add(kvp);
            ReachableStorages = ReachableStorages.OrderBy(d => d.Value).ToList();
            CurrentState.OnNewStorageAvaliable(storageBuilding);
            if (UtilityDebug.ActivebuildingLog)
                Debug.Log($"[ActiveBuilding] {BuildingName} склад {storageBuilding.BuildingName} добавлен в список доступных. [{Time.deltaTime}]", gameObject);
        }
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[ActiveBuilding] {BuildingName} склад {storageBuilding.BuildingName} недостижим. [{Time.deltaTime}]", gameObject);
    }

    public override void finishConstruction()
    {
        AB_State_ProductionCycle newState = new AB_State_ProductionCycle(this);
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[ActiveBuilding] {BuildingName} здание достроилось, меняю стейт на [AB_Sate_ProductionCycle]" +
                  $" [{Time.deltaTime}]", gameObject);
        changeState(newState);
    }


}
