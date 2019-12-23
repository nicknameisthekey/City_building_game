using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AB_State_CollectingMaterials : ActiveBuildingState
{
    public event Action<ActiveBuildingState> StateChanged = delegate { };
    public Dictionary<RecourceType, int> recourcesLeftToDeliver { get; private set; } = new Dictionary<RecourceType, int>();
    public event Action<AB_State_CollectingMaterials> RecourcesLeftChanged = delegate { };
    public AB_State_CollectingMaterials(ActiveBuildingNew activeBuilding)
    {
        building = activeBuilding;
    }

    public override void Initialize()
    {
        building.updateReachableStorages();

        foreach (var r in building.AbParams.ConstructRecources)
            recourcesLeftToDeliver.Add(r.Type, r.Amount);

        if (building.ReachableStorages != null)
        {
            if (AskAllStoragesForRecources())
            {
                Debug.Log("собрали все ресурсы, смена стейта по первичной проверке");
                unsubscribeFromStorages();
                changeToNextState();
                return;
            }
            else
            {
                foreach (var st in building.ReachableStorages)
                {
                    st.Key.Storage.RecourcesChanged += onStorageRecourcesChanged;
                }
            }
            checkRecourcesCollected();
        }
    }

    public override void OnNewStorageAvaliable(StorageBuilding storageBuilding)
    {
        //не оптимально, спрашивать только новый склад, а на все заново
        storageBuilding.Storage.RecourcesChanged += onStorageRecourcesChanged;
        AskAllStoragesForRecources();
    }

    //вызывается при инициализации
    bool AskAllStoragesForRecources()
    {
        foreach (var storageBuilding in building.ReachableStorages)
        {
            List<KeyValuePair<RecourceType, int>> kvpairs = recourcesLeftToDeliver.ToList();
            foreach (var recource in kvpairs)
            {
                requestRecources(recource.Key, recource.Value, storageBuilding.Key.Storage);
            }
        }
        if (recourcesLeftToDeliver.Count == 0) return true;
        else return false;
    }
    //запрос ресурса у конкретного склада
    void requestRecources(RecourceType type, int amount, Storage storage)
    {
        if (storage.SubstractMaximumAmount(type, amount, out int changed))
        {
            recourcesLeftToDeliver.Remove(type);
            RecourcesLeftChanged.Invoke(this);
            // Debug.Log("Получил все необходимые " + type);
        }
        else if (changed >= 0)
        {
            recourcesLeftToDeliver[type] -= changed;
            RecourcesLeftChanged.Invoke(this);
            // Debug.Log("Запросил со склада для стройки " + changed + " " + type);
        }
    }
    //проверка при изменении на складе
    void onStorageRecourcesChanged(Storage storage)
    {
        List<KeyValuePair<RecourceType, int>> kvpairs = recourcesLeftToDeliver.ToList();
        foreach (var recource in kvpairs)
        {
            requestRecources(recource.Key, recource.Value, storage);
        }
        checkRecourcesCollected();
    }
    void checkRecourcesCollected()
    {
        if (recourcesLeftToDeliver.Count == 0)
        {
            Debug.Log("все ресурсы доставлены, выход по проверке");
            unsubscribeFromStorages();
            changeToNextState();
        }
    }

    void unsubscribeFromStorages()
    {
        foreach (var st in building.ReachableStorages)
            st.Key.Storage.RecourcesChanged -= onStorageRecourcesChanged;
    }

    void changeToNextState()
    {
        AB_State_ProductionCycle newState = new AB_State_ProductionCycle(building);
        building.ChangeState(newState);
        StateChanged.Invoke(newState);
    }
}
