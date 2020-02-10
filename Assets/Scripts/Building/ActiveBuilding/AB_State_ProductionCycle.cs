using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_State_ProductionCycle : BuildingState
{
    ActiveBuildingParams abParams;
    public AB_Work Ab_Work { get; private set; }
    public AB_Output Ab_Output { get; private set; }
    public AB_Input AB_Input { get; private set; }
    public Dictionary<RecourceType, int> InputRecourcesLocal { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecourcesLocal { get; private set; } = new Dictionary<RecourceType, int>();
    public bool StaticRecourceProvided_bool { get; private set; } = false;
    public event Action OutputAdded = delegate { };
    public event Action OutputSubstracted = delegate { };
    public event Action InputSubstracted = delegate { };
    public event Action InputAdded = delegate { };
    public event Action StaticRecourcesProvided = delegate { };
    public new ActiveBuilding Building;

    public AB_State_ProductionCycle(ActiveBuilding activeBuilding)
    {
        Building = activeBuilding;
    }

    public override void Initialize()
    {
        abParams = Building.AbParams;
        fillDictionaries();
        AB_Input = new AB_Input(this);
        Ab_Work = new AB_Work(this);
        Ab_Output = new AB_Output(this);
        checkStaticRecources();
        
    }
    void staticRecourceProvided()
    {
        StaticRecourceProvided_bool = true;
        if (abParams.InputRequired)
            AB_Input.Initialize();
        Ab_Work.Initialization();
    }
    void fillDictionaries()
    {
        if (Building.AbParams.InputRequired)
            foreach (var kvp in abParams.InputRecources)
                InputRecourcesLocal.Add(kvp.Key, 0);
        foreach (var kvp in abParams.OutputRecources)
            OutputRecourcesLocal.Add(kvp.Key, 0);
    }
    public void substractInput()
    {
        foreach (var res in abParams.InputRecources)
        {
            InputRecourcesLocal[res.Key] -= res.Value;
            Debug.Log(Building.BuildingName + " вычитание в прод.цикле <color=purple>пустил в работу " + res.Value + " " + res.Key +
                    " осталось на складе " + InputRecourcesLocal[res.Key] + "</color>");
            if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_State_ProductionCycle] {Building.BuildingName} Пустил в работу [{res.Value} {res.Key}]" +
                                $" осталось на складе [{InputRecourcesLocal[res.Key]}] [{Time.deltaTime}]", Building.gameObject);
        }
        InputSubstracted.Invoke();
    }
    public void addOutputInvoke()
    {
        OutputAdded.Invoke();
    }
    public void SubstractOutput(RecourceType type, int amount)
    {
        OutputRecourcesLocal[type] -= amount;
        OutputSubstracted.Invoke();
    }

    public override void OnNewStorageAvaliable(StorageBuilding storageBuilding)
    {
        Ab_Output.OnNewStorageAvaliable(storageBuilding.Storage);
    }

    public override void StartStopWork()
    {
        Ab_Work.StartStop();
    }

    void checkStaticRecources()
    {
        StaticRecources.RecourcesChanged -= checkStaticRecources;
        if (StaticRecources.CanChangeAmount(abParams.StaticRecourceCost, true))
        {
            StaticRecources.SubstractRecources(abParams.StaticRecourceCost);
            if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_State_productionCycle] {Building.BuildingName} проверка статичных ресурсов пройдена" +
                  $" [{Time.deltaTime}]", Building.gameObject);
            staticRecourceProvided();
        }
        else
            StaticRecources.RecourcesChanged += checkStaticRecources;
    }
}
