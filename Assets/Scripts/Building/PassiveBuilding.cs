using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveBuilding : BuildingNearRoad
{
    [SerializeField] PassiveBuildingParams _pbParams;
    public PassiveBuildingParams PBParams { get => _pbParams; }

    public override void changeState(BuildingState newstate)
    {
        throw new NotImplementedException();
    }

    public override void finishConstruction()
    {
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} закончил строительство, меняю стейт на [тут будет название стейта]" +
                   $" [{Time.deltaTime}]", gameObject);
        tryChangeStaticRecources();
    }

    public override void Initialize(Vector2Int tileID, Vector2Int roadConnectionPoint)
    {
        base.Initialize(tileID, roadConnectionPoint);
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} инициализирован в {tileID}, прилегающая дорога " +
            $"{roadConnectionPoint}, нашел [{ReachableStorages.Count}] достижимых складов. [{Time.deltaTime}]", gameObject);
        CurrentState = new State_Construction(this);
        CurrentState.Initialize();
        Map.NewStorageBuildingPlaced += onNewStorageBuild;
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
                Debug.Log($"[PassiveBuilding] {BuildingName} склад {storageBuilding.BuildingName} добавлен в список доступных. [{Time.deltaTime}]", gameObject);
        }
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[PassiveBuilding] {BuildingName} склад {storageBuilding.BuildingName} недостижим. [{Time.deltaTime}]", gameObject);
    }

    void tryChangeStaticRecources()
    {
        StaticRecources.RecourcesChanged -= tryChangeStaticRecources;
        if (PBParams.recourcesRequired)
        {
            if (StaticRecources.ChangeAmount(PBParams.StaticRecourcesRequired))
                StaticRecources.ChangeAmount(PBParams.StaticRecourcesProvided);
            else
                StaticRecources.RecourcesChanged += tryChangeStaticRecources;
        }
        else
            StaticRecources.ChangeAmount(PBParams.StaticRecourcesProvided);
    }
}
