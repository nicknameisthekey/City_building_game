using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveBuilding : BuildingNearRoad
{
    [SerializeField] float TimeToTick;

    float timeElapsedFromTick;
    Coroutine tickingCour;
    enum productionStates { startCycle, inputUnavaliable, startProduction, inProduction, productionFinished, outputUnavaliable }
    productionStates currentState = productionStates.startCycle;
    List<KeyValuePair<StorageBuilding, int>> reachableStorages;
    [SerializeField] List<RecourceType> recourceTypes;
    [SerializeField] List<int> amounts;
    Dictionary<RecourceType, int> inputRecources = new Dictionary<RecourceType, int>();
    Dictionary<RecourceType, int> outputRecources = new Dictionary<RecourceType, int>();
    bool inputRequired = false;
    StorageBuilding currentStorage;
    private void Awake()
    {
        checkIfAllSetRight();
        fillRecourcesDictionaries();
    }
    void fillRecourcesDictionaries()
    {
        for (int i = 0; i < recourceTypes.Count; i++)
        {
            if (amounts[i] >= 0)
                outputRecources.Add(recourceTypes[i], amounts[i]);
            else
            {
                inputRecources.Add(recourceTypes[i], amounts[i]);
                inputRequired = true;
            }

        }
    }
    public override void Initialize(Vector2Int tileID, Vector2Int roadConntectionPoint)
    {
        base.Initialize(tileID, roadConntectionPoint);
        Map.NewStorageBuildingPlaced += checkNewStorageBuildingForPathAvaliable;
        findReachableStorages();
        productionCycle();
    }
    void findReachableStorages()
    {
        Dictionary<StorageBuilding, int> reachableStoragesUnsorted = new Dictionary<StorageBuilding, int>();
        foreach (var st in Map.StorageBuildings)
        {
            var path = Pathfinding.FindPath(RoadIDItConnects, st.RoadIDItConnects);
            if (path != null)
                reachableStoragesUnsorted.Add(st, path.Count);
        }
        reachableStorages = reachableStoragesUnsorted.OrderBy(d => d.Value).ToList();
    }
    void checkNewStorageBuildingForPathAvaliable(StorageBuilding storageBuilding)
    {
        var path = Pathfinding.FindPath(_roadIDItConnects, storageBuilding.RoadIDItConnects);
        if (path != null)
        {
            KeyValuePair<StorageBuilding, int> kvp = new KeyValuePair<StorageBuilding, int>(storageBuilding, path.Count);
            reachableStorages.Add(kvp);
            reachableStorages = reachableStorages.OrderBy(d => d.Value).ToList();
            onStorageRecourcesChanged(new Storage());
        }
    }
    StorageBuilding getAvaliableStorage(Dictionary<RecourceType, int> recourcesToLook)
    {
        foreach (var kvp in reachableStorages)
        {
            if (kvp.Key.Storage.CanChangeRecources(recourcesToLook))
            {
                //Debug.Log("в тру");
                return kvp.Key;
            }

        }
        return null;
    }
    void productionCycle()
    {
        switch (currentState)
        {
            case productionStates.startCycle:
                {
                    if (inputRequired)
                    {
                        var storage = getAvaliableStorage(inputRecources);
                        if (storage != null)
                        {
                            storage.Storage.ChangeRecources(inputRecources);
                            currentState = productionStates.startProduction;
                        }
                        else
                        {
                            currentState = productionStates.inputUnavaliable;
                            subscribeForStorageRecourcesChanged(true);
                            return;
                        }
                    }
                    else
                        currentState = productionStates.startProduction;
                    productionCycle();
                    break;
                }
            case productionStates.inputUnavaliable:
                {

                    break;
                }
            case productionStates.startProduction:
                {
                    currentState = productionStates.inProduction;
                    StartCoroutine(timerCour());
                    break;
                }
            case productionStates.productionFinished:
                {
                    var storage = getAvaliableStorage(outputRecources);
                    if (storage != null)
                    {
                        storage.Storage.ChangeRecources(outputRecources);
                        currentState = productionStates.startCycle;
                        productionCycle();
                    }
                    else
                    {
                        currentState = productionStates.outputUnavaliable;
                        subscribeForStorageRecourcesChanged(true);
                    }
                    break;
                }
            default: break;
        }
    }
    void subscribeForStorageRecourcesChanged(bool subscribe)
    {
        if (subscribe)
            foreach (var kvp in reachableStorages)
                kvp.Key.Storage.RecourcesChanged += onStorageRecourcesChanged;
        else
            foreach (var kvp in reachableStorages)
                kvp.Key.Storage.RecourcesChanged -= onStorageRecourcesChanged;
    }

    void onStorageRecourcesChanged(Storage storage)
    {
        subscribeForStorageRecourcesChanged(false);
        if (currentState == productionStates.inputUnavaliable)
        {
            currentState = productionStates.startCycle;
            productionCycle();
            return;
        }
        else if (currentState == productionStates.outputUnavaliable)
        {
            currentState = productionStates.productionFinished;
            productionCycle();
            return;
        }
    }
    IEnumerator timerCour()
    {
        //  Debug.Log("на старте" + timeElapsedFromTick);
        while (timeElapsedFromTick <= TimeToTick)
        {
            //  Debug.Log("в цикле " + timeElapsedFromTick);
            timeElapsedFromTick++;
            yield return new WaitForSeconds(1f);
        }
        // Debug.Log("вышел из корутины");
        timeElapsedFromTick = 0;
        currentState = productionStates.productionFinished;
        productionCycle();
    }

    void checkIfAllSetRight()
    {
        if (TimeToTick <= 0)
            Debug.Log("time to tick was not set for " + gameObject.name);
    }
    private void OnDestroy()
    {
        Map.NewStorageBuildingPlaced -= checkNewStorageBuildingForPathAvaliable;
    }
}
