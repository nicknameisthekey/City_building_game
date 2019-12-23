using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_State_ProductionCycle : ActiveBuildingState
{
    ActiveBuildingParams abParams;
    public AB_Work ab_Work { get; private set; }
    AB_Output ab_Output;
    public Dictionary<RecourceType, int> InputRecourcesLocal { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> OutputRecourcesLocal { get; private set; } = new Dictionary<RecourceType, int>();
    public event Action OutputAdded = delegate { };
    public event Action OutputSubstracted = delegate { };


    public AB_State_ProductionCycle(ActiveBuildingNew activeBuilding)
    {
        building = activeBuilding;
    }

    public override void Initialize()
    {
        abParams = building.AbParams;
        fillDictionaries();
        ab_Work = new AB_Work(this);
        ab_Work.Initialization();
        ab_Output = new AB_Output(this);
        ab_Output.Initialization();
    }
    void fillDictionaries()
    {
        if (building.AbParams.InputRequired)
            foreach (var kvp in abParams.InputRecources)
                InputRecourcesLocal.Add(kvp.Key, kvp.Value);
        foreach (var kvp in abParams.OutputRecources)
            OutputRecourcesLocal.Add(kvp.Key, 0);
    }
    public void addOutput()
    {
        foreach (var kvp in abParams.OutputRecources)
        {
            OutputRecourcesLocal[kvp.Key] += kvp.Value;
            Debug.Log($"<color=green>отправил " + kvp.Key + " в количестве " + kvp.Value + " в локальный склад, всего</color> " + OutputRecourcesLocal[kvp.Key]);
        }
        OutputAdded.Invoke();
    }
    public void SubstractOutput(RecourceType type, int amount)
    {
        Debug.Log("отправил " + type + " в количестве " + amount + " на чужой склад, всего было " + OutputRecourcesLocal[type]);
        OutputRecourcesLocal[type] -= amount;
        OutputSubstracted.Invoke();
    }

    public override void OnNewStorageAvaliable(StorageBuilding storageBuilding)
    {
        ab_Output.OnNewStorageAvaliable(storageBuilding.Storage);
    }
}
