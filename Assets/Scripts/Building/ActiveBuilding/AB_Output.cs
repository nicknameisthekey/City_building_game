using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AB_Output
{
    AB_State_ProductionCycle state;
    ActiveBuildingParams abParams;
    public bool ProductionFlag { get; private set; } = true;
    public event Action ProductionAvaliable = delegate { };
    event Action localStorageAdd = delegate { };

    bool storageEmpty = true;
    public AB_Output(AB_State_ProductionCycle state)
    {
        this.state = state;
        abParams = state.Building.AbParams;
        localStorageAdd += onLocalStorageAdd;
    }

    //проход по всем складам в попытке отдать ресурсы
    //отправка на удаленный склад текущего output и остатков на локальный склад
    public void AddOutput()
    {
        List<Recource> outputNotSent = new List<Recource>();
        foreach (var res in abParams.OutputRecources)
            outputNotSent.Add(new Recource(res.Key, res.Value));

        foreach (var st in state.Building.ReachableStorages)
        {
            if (outputNotSent.Count == 0) return;
            for (int i = 0; i < outputNotSent.Count; i++)
            {
                if (st.Key.Storage.AddMaximumAmount(outputNotSent[i].Type, outputNotSent[i].Amount, out int changed))
                {
                    if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} отправил на склад {st.Key.BuildingName} " +
           $" полностью [{outputNotSent[i].Amount} {outputNotSent[i].Type}] [{ Time.deltaTime}]", state.Building.gameObject);
                    outputNotSent.RemoveAt(i);
                }
                else if (changed != 0)
                {
                    outputNotSent[i].Amount -= changed;
                    if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} отправил на склад {st.Key.BuildingName} " +
            $" [{changed} {outputNotSent[i].Type}], осталось отправить {outputNotSent[i].Amount} [{ Time.deltaTime}]", state.Building.gameObject);
                }
            }
        }
        addOnLocalStorge(outputNotSent);
    }

    //работа с локальным складом
    void addOnLocalStorge(List<Recource> recources)
    {
        foreach (var res in recources)
        {
            state.OutputRecourcesLocal[res.Type] += res.Amount;
            if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} на локальный склад пришло" +
            $"[{res.Amount} {res.Type}], всего стало {state.OutputRecourcesLocal[res.Type]} [{ Time.deltaTime}]", state.Building.gameObject);
        }
        state.addOutputInvoke();
        localStorageAdd.Invoke();
        updateProductionFlag();
        updateLocalStorageEmptyFlag();
    }

    void updateProductionFlag()
    {
        foreach (var res in state.OutputRecourcesLocal)
        {
            if (res.Value + abParams.OutputRecources[res.Key] > abParams.OutputRecourceCapacity[res.Key])
            {
                ProductionFlag = false;
                if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} обновил production flag {ProductionFlag}" +
                    $" [{ Time.deltaTime}]", state.Building.gameObject);
                return;
            }
        }
        ProductionFlag = true;
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} обновил production flag {ProductionFlag}" +
                    $" [{ Time.deltaTime}]", state.Building.gameObject);
        ProductionAvaliable.Invoke();
    }

    //output часть

    //onlocal слушает добавление на локальный склад только когда склад пуст и включает прослушку всех складов
    void onLocalStorageAdd()
    {
        localStorageAdd -= onLocalStorageAdd;
        foreach (var st in state.Building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged += trySentToStorage;
        }
    }
    //отправляет на склад и проверяет пуст ли локальный склад после
    void trySentToStorage(Storage storage)
    {
        var kvps = state.OutputRecourcesLocal.ToList();
        foreach (var res in kvps)
        {
            if (res.Value == 0) break;
            storage.RecourcesChanged -= trySentToStorage;
            storage.AddMaximumAmount(res.Key, res.Value, out int changed);
            storage.RecourcesChanged += trySentToStorage;
            if (changed != 0)
            {
                state.SubstractOutput(res.Key, changed);
                if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} на локальном складе было [{res.Value} {res.Key}]" +
                    $" отправил {changed}, стало {state.OutputRecourcesLocal[res.Key]} [{ Time.deltaTime}]", state.Building.gameObject);
            }
        }
        updateLocalStorageEmptyFlag();
        updateProductionFlag();
    }
    //апдейт флага
    void updateLocalStorageEmptyFlag()
    {
        foreach (var res in state.OutputRecourcesLocal)
        {
            if (res.Value != 0)
            {
                storageEmpty = false;
                if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} output склад не пуст, меняю storageEmpty на фолс" +
                    $" на складе есть [{res.Value} {res.Key}] [{ Time.deltaTime}]", state.Building.gameObject);
                return;
            }
        }
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} output склад пуст, меняю storageEmpty на тру" +
    $" [{ Time.deltaTime}]", state.Building.gameObject);
        foreach (var st in state.Building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged -= trySentToStorage;
        }
        localStorageAdd += onLocalStorageAdd;
        storageEmpty = true;
    }

    public void OnNewStorageAvaliable(Storage storage)
    {
        if (UtilityDebug.ActivebuildingLog) Debug.Log($"[AB_Output] {state.Building.BuildingName} onNewStorage, локальный склад пуст? {storageEmpty}" +
    $"  [{ Time.deltaTime}]", state.Building.gameObject);
        if (!storageEmpty)
        {
            storage.RecourcesChanged += trySentToStorage;
            trySentToStorage(storage);
        }
    }


}
