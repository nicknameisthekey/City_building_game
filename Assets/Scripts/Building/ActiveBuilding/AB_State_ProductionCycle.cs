using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_State_ProductionCycle : ActiveBuildingState
{
    ActiveBuildingParams abParams;
    public AB_Work Ab_Work { get; private set; }
    public AB_Output Ab_Output { get; private set; }
    public AB_Input AB_Input { get; private set; }
    public Dictionary<RecourceType, int> InputRecourcesLocal { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecourcesLocal { get; private set; } = new Dictionary<RecourceType, int>();
    public event Action OutputAdded = delegate { };
    public event Action OutputSubstracted = delegate { };
    public event Action InputSubstracted = delegate { };
    public event Action InputAdded = delegate { };


    public AB_State_ProductionCycle(ActiveBuildingNew activeBuilding)
    {
        building = activeBuilding;
    }

    public override void Initialize()
    {
        abParams = building.AbParams;
        fillDictionaries();
        AB_Input = new AB_Input(this);
        Ab_Work = new AB_Work(this);
        Ab_Output = new AB_Output(this);
        if (abParams.InputRequired)
            AB_Input.Initialize();
        Ab_Work.Initialization();

    }
    void fillDictionaries()
    {
        if (building.AbParams.InputRequired)
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
            Debug.Log(building.BuildingName + " вычитание в прод.цикле <color=purple>пустил в работу " + res.Value + " " + res.Key +
                    " осталось на складе " + InputRecourcesLocal[res.Key] + "</color>");
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
}
