using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveBuilding : BuildingNearRoad, ITooltipConstruction
{
    [SerializeField] PassiveBuildingParams _pbParams;
    public PassiveBuildingParams PBParams { get => _pbParams; }

    public override void changeState(BuildingState newState)
    {
        CurrentState = newState;
        newState.Initialize();
        stateChangedInvoke(newState);
    }

    public override void finishConstruction()
    {
        if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} закончил строительство, меняю стейт на [PB_ProductionCycle]" +
                   $" [{Time.deltaTime}]", gameObject);
        changeState(new PB_ProductionCycle(this));
    }

    public override void Initialize(Vector2Int tileID, Vector2Int roadConnectionPoint)
    {
        base.Initialize(tileID, roadConnectionPoint);
        if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} инициализирован в {tileID}, прилегающая дорога " +
            $"{roadConnectionPoint}, нашел [{ReachableStorages.Count}] достижимых складов. [{Time.deltaTime}]", gameObject);
        CurrentState = new State_Construction(this);
        CurrentState.Initialize();
        Map.NewStorageBuildingPlaced += onNewStorageBuild;
    }

    public object GetTooltipModel()
    {
        return new PB_BuildingMenu_TooltipModel(PBParams);
    }

    protected override void onNewStorageBuild(StorageBuilding storageBuilding)
    {
        if (CurrentState is BuildingState)
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
                if (UtilityDebug.PassivebuildingLog)
                    Debug.Log($"[PassiveBuilding] {BuildingName} склад {storageBuilding.BuildingName} добавлен в список доступных. [{Time.deltaTime}]", gameObject);
            }
            if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} склад {storageBuilding.BuildingName} недостижим. [{Time.deltaTime}]", gameObject);
        }
        else
            if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} игнорирование нового склада {storageBuilding.BuildingName}, стейт не строительство. [{Time.deltaTime}]", gameObject);
    }


}
