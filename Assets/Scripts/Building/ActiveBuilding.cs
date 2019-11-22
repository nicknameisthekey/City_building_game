using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuilding : Building
{
    [SerializeField] float TimeToTick;
    [SerializeField] Vector2Int _roadConnectionPoint;
    float timeElapsedFromTick;
    public Vector2Int RoadConnectionPoint { get => _roadConnectionPoint; }
    Coroutine tickingCour;
    [SerializeField] RecourceType recource;
    [SerializeField] float amount;
    private void Awake()
    {
        checkIfAllSetRight();
    }
    public override void Initialize(Vector2Int tileID)
    {
        base.Initialize(tileID);
        Vector2Int connectedRoadID = TileID + _roadConnectionPoint;
        var roadTile = MapGenerator.Map[connectedRoadID.x, connectedRoadID.y] as TileWithBuilding;
        Road road = roadTile.Building as Road;
        if (road.currentNetwork == null)
        {
            Debug.Log("road network null");
        }
        ChangeStorage(road.currentNetwork);
        startTicking();
    }
    void startTicking()
    {
        if (tickingCour == null && currentNetwork != null)
        {
            Debug.Log("запускаю корутину");
            tickingCour = StartCoroutine(tickCour());
        }
        else
        {
            Debug.Log("cour already started or storage null");
        }
    }
    IEnumerator tickCour()
    {
        while (true)
        {
            timeElapsedFromTick++;
            if (timeElapsedFromTick > TimeToTick)
            {
                tick();
                timeElapsedFromTick = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    void tick()
    {
        currentNetwork.changeRecourceAmount(recource, amount);
        //  Debug.Log("Изменил " + recource.ToString() + " на " + amount + " стало " + currentNetwork.Recources[recource]);
    }
    public override void ChangeStorage(RoadNetwork newStorage)
    {
        if (currentNetwork != newStorage)
        {
            currentNetwork = newStorage;
            currentNetwork.addBuildingToNetwork(this);
            startTicking();
        }
    }

    void checkIfAllSetRight()
    {
        if (TimeToTick <= 0)
            Debug.Log("time to tick was not set for " + gameObject.name);
    }
}
