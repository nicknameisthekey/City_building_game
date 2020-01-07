using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Input
{
    AB_State_ProductionCycle state;
    ActiveBuildingParams abParams;
    //List<Recource> freeSpaceLeft = new List<Recource>();
    public bool ProductionFlag { get; private set; } = true; //по умолчанию тру, фолс после инициализации
    public event Action ProductionAvaliable = delegate { };
    bool storageFull = false;
    public AB_Input(AB_State_ProductionCycle state)
    {
        this.state = state;
        abParams = state.building.AbParams;
    }
    public void Initialize() { ProductionFlag = false; requestRecourcesFromAllStorages(); }

    public void requestRecourcesFromAllStorages()
    {
        state.InputSubstracted -= requestRecourcesFromAllStorages;
        foreach (var st in state.building.ReachableStorages)
        {
            foreach (var res in abParams.InputRecourceCapacity)
            {
                st.Key.Storage.SubstractMaximumAmount(res.Key, res.Value - state.InputRecourcesLocal[res.Key], out int changed);
                if (changed != 0)
                {
                    Debug.Log(res.Value);
                    state.InputRecourcesLocal[res.Key] += changed;
                    Debug.Log(state.building.BuildingName +" <color=blue>взял на локальный склад " + changed + " " + res.Key +
                        " осталось до заполнения " + (res.Value - state.InputRecourcesLocal[res.Key]) + "</color>");
                }
            }
        }
        updateProductionFlag();
        checkStorageFull();
    }
    void subscribeForStorages()
    {
        foreach (var st in state.building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged += requestRecourcesFromStorage;
        }
    }
    void unSubscribeForStorages()
    {
        foreach (var st in state.building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged -= requestRecourcesFromStorage;
        }
    }
    void requestRecourcesFromStorage(Storage st)
    {
        foreach (var res in abParams.InputRecourceCapacity)
        {
            st.SubstractMaximumAmount(res.Key, res.Value - state.InputRecourcesLocal[res.Key], out int changed);
            if (changed != 0)
            {
                state.InputRecourcesLocal[res.Key] += changed;
                Debug.Log(state.building.BuildingName + "<color=blue> input взял на локальный склад " + changed + " " + res.Key +
                    " осталось до заполнения " + (res.Value - state.InputRecourcesLocal[res.Key]) + "</color>");
            }
        }
        updateProductionFlag();
        checkStorageFull();
    }
    void checkStorageFull()
    {
        storageFull = true;
        foreach (var res in state.InputRecourcesLocal)
        {
            if (res.Value != abParams.InputRecourceCapacity[res.Key])
            {
                storageFull = false;
                unSubscribeForStorages(); //подпишится после
            }
        }
        if (storageFull)
        {
            state.InputSubstracted += requestRecourcesFromAllStorages;
            unSubscribeForStorages();
        }
        else
        {
            subscribeForStorages();
        }
    }

    void updateProductionFlag()
    {
        foreach (var res in state.InputRecourcesLocal)
        {
            if (res.Value < abParams.InputRecources[res.Key])
            {
                ProductionFlag = false;
                return;
            }
        }
        Debug.Log(state.building.BuildingName + " input разрешил производство");
        ProductionFlag = true;
        ProductionAvaliable.Invoke();
    }
    public void OnNewStorage(Storage storage)
    {
        if (!storageFull)
        {
            storage.RecourcesChanged += requestRecourcesFromStorage;
            requestRecourcesFromStorage(storage);
        }
    }
}
