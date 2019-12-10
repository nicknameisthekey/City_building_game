using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveBuilding : BuildingNearRoad
{
    [SerializeField] float TimeToTick;

    float timeElapsedFromTick;
    Coroutine tickingCour;

    List<KeyValuePair<StorageBuilding, int>> avaliableStorages;
    bool recourceSentToStorage = true;
    [SerializeField] RecourceType recource;
    [SerializeField] float amount;
    StorageBuilding currentStorage;
    private void Awake()
    {
        checkIfAllSetRight();
    }
    public override void Initialize(Vector2Int tileID, Vector2Int roadConntectionPoint)
    {
        base.Initialize(tileID, roadConntectionPoint);
        Map.NewStorageBuildingPlaced += checkNewStorageBuildingForPathAvaliable;
        findAvaliableStorages();
        startTicking();
    }
    void findAvaliableStorages()
    {
        Dictionary<StorageBuilding, int> avaliableStoragesUnsorted = new Dictionary<StorageBuilding, int>();
        foreach (var st in Map.StorageBuildings)
        {
            var path = Pathfinding.FindPath(RoadIDItConnects, st.RoadIDItConnects);
            if (path != null)
                avaliableStoragesUnsorted.Add(st, path.Count);
        }
        avaliableStorages = avaliableStoragesUnsorted.OrderBy(d => d.Value).ToList();
    }
    void checkNewStorageBuildingForPathAvaliable(StorageBuilding storageBuilding)
    {
        var path = Pathfinding.FindPath(_roadIDItConnects, storageBuilding.RoadIDItConnects);
        if (path != null)
        {
            KeyValuePair<StorageBuilding, int> kvp = new KeyValuePair<StorageBuilding, int>(storageBuilding, path.Count);
            avaliableStorages.Add(kvp);
            avaliableStorages = avaliableStorages.OrderBy(d => d.Value).ToList();
            if (recourceSentToStorage == false)
                tick();
        }
    }
    StorageBuilding getStorageToStoreRecource()
    {
        foreach (var kvp in avaliableStorages)
        {
            if (kvp.Key.Storage.CanAddRecource(recource, amount))
                return kvp.Key;
        }
        return null;
    }
    void startTicking()
    {
        if (tickingCour == null && recourceSentToStorage)
        {
            Debug.Log("запускаю корутину");
            tickingCour = StartCoroutine(tickCour());
        }
        else
        {
            Debug.Log("cour already started or recources was not sent");
        }
    }
    IEnumerator tickCour()
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
        tick();
    }
    void tick()
    {
        recourceSentToStorage = false;
        tickingCour = null;
        var storage = getStorageToStoreRecource();
        if (storage != null)
        {
            storage.Storage.AddRecource(recource, amount);
            recourceSentToStorage = true;
            // Debug.Log("tick");
            startTicking();
        }
        else
        {
            Debug.Log("нет доступных складов");
        }
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
