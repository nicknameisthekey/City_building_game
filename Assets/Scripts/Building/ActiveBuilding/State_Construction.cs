using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State_Construction : BuildingState
{
    public event Action<State_Construction> RecourcesLeftChanged = delegate { };
    public Dictionary<RecourceType, int> RecourcesLeftToDeliver { get; private set; } = new Dictionary<RecourceType, int>();

    BuildingParams buildingParams; 
    public State_Construction(BuildingNearRoad building)
    {
        Building = building;
        if (building is ActiveBuilding activeBuilding)
            buildingParams = activeBuilding.AbParams;
        if (building is PassiveBuilding passiveBuilding)
            buildingParams = passiveBuilding.PBParams;
    }
    public override void Initialize()
    {
        foreach (var r in buildingParams.ConstructRecources)
            RecourcesLeftToDeliver.Add(r.Type, r.Amount);

        if (Building.ReachableStorages != null)
        {
            if (AskAllStoragesForRecources())
            {
                if (UtilityDebug.BuildingConstructionLog) Debug.Log($"[State_Construction] {Building.BuildingName} собрал все ресурсы" +
                    $" в инициализации [{Time.deltaTime}]", Building.gameObject);
                unsubscribeFromStorages();
                changeToNextState();
                return;
            }
            else
            {
                foreach (var st in Building.ReachableStorages)
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
        foreach (var storageBuilding in Building.ReachableStorages)
        {
            List<KeyValuePair<RecourceType, int>> kvpairs = RecourcesLeftToDeliver.ToList();
            foreach (var recource in kvpairs)
            {
                requestRecources(recource.Key, recource.Value, storageBuilding.Key.Storage);
            }
        }
        if (RecourcesLeftToDeliver.Count == 0) return true;
        else return false;
    }
    //запрос ресурса у конкретного склада
    void requestRecources(RecourceType type, int amount, Storage storage)
    {
        if (storage.SubstractMaximumAmount(type, amount, out int changed))
        {
            RecourcesLeftToDeliver.Remove(type);
            RecourcesLeftChanged.Invoke(this);
            if (UtilityDebug.BuildingConstructionLog) Debug.Log($"[State_Construction] {Building.BuildingName} получил вcе необходимые [{type}]" +
                   $" со склада {storage.Building.BuildingName} [{Time.deltaTime}]", Building.gameObject);
        }
        else if (changed >= 0)
        {
            RecourcesLeftToDeliver[type] -= changed;
            RecourcesLeftChanged.Invoke(this);
            if (UtilityDebug.BuildingConstructionLog) Debug.Log($"[State_Construction] {Building.BuildingName} получил [{changed}] [{type}]" +
                    $" со склада {storage.Building.BuildingName}, осталось [{RecourcesLeftToDeliver[type]}] [{Time.deltaTime}]", Building.gameObject);
        }
    }
    //проверка при изменении на складе
    void onStorageRecourcesChanged(Storage storage)
    {
        List<KeyValuePair<RecourceType, int>> kvpairs = RecourcesLeftToDeliver.ToList();
        foreach (var recource in kvpairs)
        {
            requestRecources(recource.Key, recource.Value, storage);
        }
        checkRecourcesCollected();
    }
    void checkRecourcesCollected()
    {
        if (RecourcesLeftToDeliver.Count == 0)
        {
            if (UtilityDebug.BuildingConstructionLog) Debug.Log($"[State_Construction] {Building.BuildingName} собрал все ресурсы" +
                   $" [{Time.deltaTime}]", Building.gameObject);
            unsubscribeFromStorages();
            changeToNextState();
        }
    }
    void unsubscribeFromStorages()
    {
        foreach (var st in Building.ReachableStorages)
            st.Key.Storage.RecourcesChanged -= onStorageRecourcesChanged;
    }
    void changeToNextState()
    {
       
        Building.finishConstruction();
    }
    public override void StartStopWork()
    {
        if (UtilityDebug.BuildingConstructionLog) Debug.Log($"[State_Construction] {Building.BuildingName} не имплементировано [{Time.deltaTime}]", Building.gameObject);
    }
}
