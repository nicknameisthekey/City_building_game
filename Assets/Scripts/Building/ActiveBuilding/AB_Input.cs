using System;
using UnityEngine;

public class AB_Input
{
    AB_State_ProductionCycle state;
    ActiveBuildingParams abParams;
    public bool ProductionFlag { get; private set; } = true; //по умолчанию тру, фолс после инициализации
    public event Action ProductionAvaliable = delegate { };
    bool storageFull = false;
    public AB_Input(AB_State_ProductionCycle state)
    {
        this.state = state;
        abParams = state.Building.AbParams;
    }
    public void Initialize() { ProductionFlag = false; requestRecourcesFromAllStorages(); }

    public void requestRecourcesFromAllStorages()
    {
        state.InputSubstracted -= requestRecourcesFromAllStorages;
        foreach (var st in state.Building.ReachableStorages)
        {
            foreach (var res in abParams.InputRecourceCapacity)
            {
                st.Key.Storage.SubstractMaximumAmount(res.Key, res.Value - state.InputRecourcesLocal[res.Key], out int changed);
                if (changed != 0)
                {
                    state.InputRecourcesLocal[res.Key] += changed;
                    if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Input] {state.Building.BuildingName} взял на локальный склад" +
                        $" [{changed} {res.Key}], осталось до заполнения { (res.Value - state.InputRecourcesLocal[res.Key])}" +
                        $" [{ Time.deltaTime}]", state.Building.gameObject);

                }
            }
        }
        updateProductionFlag();
        checkStorageFull();
    }
    void subscribeForStorages()
    {
        foreach (var st in state.Building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged += requestRecourcesFromStorage;
        }
    }
    void unSubscribeForStorages()
    {
        foreach (var st in state.Building.ReachableStorages)
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
                if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Input] {state.Building.BuildingName} взял на локальный склад" +
                        $" [{changed} {res.Key}], осталось до заполнения { (res.Value - state.InputRecourcesLocal[res.Key])}" +
                        $" [{ Time.deltaTime}]", state.Building.gameObject);
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
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Input] {state.Building.BuildingName} разрешил производство " +
            $"[{ Time.deltaTime}]", state.Building.gameObject);
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
