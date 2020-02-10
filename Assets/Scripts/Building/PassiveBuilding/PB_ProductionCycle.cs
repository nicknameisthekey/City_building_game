using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_ProductionCycle : BuildingState
{
    public PassiveBuildingParams PBParams { get; private set; }
    public event Action StaticRecourcesChanged = delegate { };
    public PB_ProductionCycle(PassiveBuilding building) { Building = building; PBParams = building.PBParams; }
    public override void Initialize()
    {
        if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PB_ProductionCycle] {Building.BuildingName} инициализирован. [{Time.deltaTime}]", Building.gameObject);
        tryChangeStaticRecources();
    }
    void tryChangeStaticRecources()
    {
        StaticRecources.RecourcesChanged -= tryChangeStaticRecources;
        if (PBParams.recourcesRequired)
        {
            if (StaticRecources.CanChangeAmount(PBParams.StaticRecourcesRequired, true))
            {
                StaticRecources.SubstractRecources(PBParams.StaticRecourcesRequired);
                StaticRecources.AddRecources(PBParams.StaticRecourcesProvided);
                if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PB_ProductionCycle] {Building.BuildingName} вычел input, дал output [{Time.deltaTime}]", Building.gameObject);
                StaticRecourcesChanged.Invoke();
            }
            else
                StaticRecources.RecourcesChanged += tryChangeStaticRecources;
        }
        else
        {
            StaticRecources.AddRecources(PBParams.StaticRecourcesProvided);
            if (UtilityDebug.PassivebuildingLog) Debug.Log($"[PB_ProductionCycle] {Building.BuildingName} input не нужен, отдаю output. [{Time.deltaTime}]", Building.gameObject);
            StaticRecourcesChanged.Invoke();
        }
    }

    public override void OnNewStorageAvaliable(StorageBuilding storageBuilding) { }
    public override void StartStopWork() { }
}
