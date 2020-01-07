using System;
using System.Collections;
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
        abParams = state.building.AbParams;
        localStorageAdd += onLocalStorageAdd;
    }

    //проход по всем складам в попытки отдать ресурсы
    //отправка на удаленный склад текущего output и остатков на локальный склад
    public void AddOutput()
    {
        List<Recource> outputNotSent = new List<Recource>();
        foreach (var res in abParams.OutputRecources)
            outputNotSent.Add(new Recource(res.Key, res.Value));

        foreach (var st in state.building.ReachableStorages)
        {
            if (outputNotSent.Count == 0) return;
            for (int i = 0; i < outputNotSent.Count; i++)
            {
                if (st.Key.Storage.AddMaximumAmount(outputNotSent[i].Type, outputNotSent[i].Amount, out int changed))
                {
                    outputNotSent.RemoveAt(i);
                }
                else if (changed != 0)
                {
                    outputNotSent[i].Amount -= changed;
                    Debug.Log(state.building.BuildingName + " output <color=green>смог отправить в удаленку " + changed + " " + outputNotSent[i].Type +
                        " осталось отправить " + outputNotSent[i].Amount + "</color>");
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
            Debug.Log(state.building.BuildingName +  " output <color=red>На локальный склад пришло " + res.Amount + " " + res.Type + " всего стало " +
                state.OutputRecourcesLocal[res.Type] + "</color>");
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
                Debug.Log(state.building.BuildingName +  " output обновил prod flag " + ProductionFlag);
                return;
            }
        }
        ProductionFlag = true;
        Debug.Log(state.building.BuildingName + " output обновил prod flag " + ProductionFlag);
        ProductionAvaliable.Invoke();
    }

    //output часть

    //onlocal слушает добавление на локальный склад только когда склад пуст и включает прослушку всех складов
    void onLocalStorageAdd()
    {
        Debug.Log(state.building.BuildingName + " output запускаю прослушку складов для отправки ресурсов");
        localStorageAdd -= onLocalStorageAdd;
        foreach (var st in state.building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged += trySentToStorage;
        }
    }
    //отправляет на склад и проверяет пуст ли локальный склад после
    void trySentToStorage(Storage storage)
    {
        //Debug.Log("внутри");
        var kvps = state.OutputRecourcesLocal.ToList();
        foreach (var res in kvps)
        {
            if (res.Value == 0) break;
            storage.RecourcesChanged -= trySentToStorage;
            storage.AddMaximumAmount(res.Key, res.Value, out int changed);
            storage.RecourcesChanged += trySentToStorage;
            //Debug.Log("changed " + changed);
            if (changed != 0)
            {
                state.SubstractOutput(res.Key, changed);
                Debug.Log(state.building.BuildingName +  " output <color=red> на локальном было " + res.Value + " " + res.Key + " отправил " +
                    changed + " стало " + state.OutputRecourcesLocal[res.Key] + "</color>");
            }
        }
        updateLocalStorageEmptyFlag();
        updateProductionFlag();
       // Debug.Log("вышел");
    }
    //апдейт флага
    void updateLocalStorageEmptyFlag()
    {
        foreach (var res in state.OutputRecourcesLocal)
        {
            if (res.Value != 0)
            {
                storageEmpty = false;
                Debug.Log(state.building.BuildingName + " output склад не пуст, меняю storageEmpty на фолс " + res.Value);
                return;
            }
        }
        Debug.Log(state.building.BuildingName + "output склад пуст, меняю storgaeEmpty на тру, выключаю прослушку");
        foreach (var st in state.building.ReachableStorages)
        {
            st.Key.Storage.RecourcesChanged -= trySentToStorage;
        }
        localStorageAdd += onLocalStorageAdd;
        storageEmpty = true;
    }

    public void OnNewStorageAvaliable(Storage storage)
    {
        Debug.Log(state.building.BuildingName + " output on new storage, локальный склад пуст? " + storageEmpty);
        if (!storageEmpty)
        {
            storage.RecourcesChanged += trySentToStorage;
            trySentToStorage(storage);
        }
    }


}
