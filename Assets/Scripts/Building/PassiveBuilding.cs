using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveBuilding : BuildingNearRoad
{
    [SerializeField] List<StaticRecource> staticRecources;
    [SerializeField] List<Recource> constructionCost;
    Dictionary<StaticRecourceType, int> _staticRecourcesRequired = new Dictionary<StaticRecourceType, int>();
    Dictionary<StaticRecourceType, int> _staticRecourcesProvided = new Dictionary<StaticRecourceType, int>();
    bool recourcesRequired = false;
    public Dictionary<RecourceType, int> ConstructionCost { get; private set; } = new Dictionary<RecourceType, int>();
    public Dictionary<RecourceType, int> recourcesLeftToDeliver { get; private set; } = new Dictionary<RecourceType, int>();

    public event Action<PassiveBuilding> RecourcesLeftChanged = delegate { };

    List<KeyValuePair<StorageBuilding, int>> reachableStorages;

    public override void Initialize(Vector2Int tileID, Vector2Int roadIDItConnects)
    {
        base.Initialize(tileID, roadIDItConnects);
        fillRecourcesDictionary();
        updateReachableStorages();
        if (reachableStorages != null)
        {
            if (AskAllStoragesForRecources())
            {
                Debug.Log(BuildingName + " строительство, собрали все ресурсы, работаю со статичными ресурсами");
                unsubscribeFromStorages();
                tryChangeStaticRecources();
                return;
            }
            else
            {
                foreach (var st in reachableStorages)
                {
                    st.Key.Storage.RecourcesChanged += onStorageRecourcesChanged;
                }
            }
            checkRecourcesCollected();
        }
    }
    void fillRecourcesDictionary()
    {
        foreach (var res in staticRecources)
        {
            if (res.Amount < 0)
            {
                _staticRecourcesRequired.Add(res.Type, res.Amount);
                recourcesRequired = true;
            }
            else
                _staticRecourcesProvided.Add(res.Type, res.Amount);
        }
        foreach (var res in constructionCost)
        {
            ConstructionCost.Add(res.Type, res.Amount);
            recourcesLeftToDeliver.Add(res.Type, res.Amount);
        }
    }

    public  void OnNewStorageAvaliable(StorageBuilding storageBuilding)
    {
        //не оптимально, спрашивать только новый склад, а на все заново
        storageBuilding.Storage.RecourcesChanged += onStorageRecourcesChanged;
        AskAllStoragesForRecources();
    }

    //вызывается при инициализации
    bool AskAllStoragesForRecources()
    {
        foreach (var storageBuilding in reachableStorages)
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
             Debug.Log("Получил все необходимые " + type);
        }
        else if (changed >= 0)
        {
            recourcesLeftToDeliver[type] -= changed;
            RecourcesLeftChanged.Invoke(this);
             Debug.Log("Запросил со склада для стройки " + changed + " " + type);
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
            Debug.Log(BuildingName + " строительство, все ресурсы доставлены, выход по проверке доставленых ресурсов");
            unsubscribeFromStorages();
            tryChangeStaticRecources();
        }
    }

    void unsubscribeFromStorages()
    {
        foreach (var st in reachableStorages)
            st.Key.Storage.RecourcesChanged -= onStorageRecourcesChanged;
    }

    public void updateReachableStorages()
    {
        reachableStorages = GameUtility.FindAllReachableStorages(RoadIDItConnects);
    }
    void tryChangeStaticRecources()
    {
        StaticRecources.RecourcesChanged -= tryChangeStaticRecources;
        if (recourcesRequired)
        {
            if (StaticRecources.ChangeAmount(_staticRecourcesRequired))
                StaticRecources.ChangeAmount(_staticRecourcesProvided);
            else
                StaticRecources.RecourcesChanged += tryChangeStaticRecources;
        }
        else
            StaticRecources.ChangeAmount(_staticRecourcesProvided);
    }
}
