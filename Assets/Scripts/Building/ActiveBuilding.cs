using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuilding : Building, INetwork
{
    [SerializeField] float TimeToTick;

    float timeElapsedFromTick;

    Coroutine tickingCour;
    [SerializeField] RecourceType recource;
    [SerializeField] float amount;
    [SerializeField] Vector2Int _roadConnectionPoint;
    RoadNetwork _currentNetwork;
    public RoadNetwork CurrentNetwork => _currentNetwork;
    public Vector2Int RoadConnectionPoint => _roadConnectionPoint;

    private void Awake()
    {
        checkIfAllSetRight();
    }
    public override void Initialize(Vector2Int tileID)
    {
        base.Initialize(tileID);
        Road road = GameUtility.GetRoadByID(TileID + _roadConnectionPoint);
        if (road != null)
        {
            if (road.CurrentNetwork == null)
            {
                Debug.Log("road network null");
            }
            else
            {
                _currentNetwork = road.CurrentNetwork;
                _currentNetwork.addBuildingToNetwork(this);
                startTicking();
            }
        }
    }
    void startTicking()
    {
        if (tickingCour == null && _currentNetwork != null)
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
        if (!_currentNetwork.ChangeRecourceInNetwork(recource, amount))
            Debug.Log("не смог добавить ресурс, нет места или подходящего хранилища");
        //Debug.Log("tick!");
        //Debug.Log("Изменил " + recource.ToString() + " на " + amount + " стало " + _currentNetwork.NetworkStorage.Recources[recource]);
    }


    void checkIfAllSetRight()
    {
        if (TimeToTick <= 0)
            Debug.Log("time to tick was not set for " + gameObject.name);
    }

    public void ChangeRoadNetwork(RoadNetwork newNetwork)
    {
        if (_currentNetwork == newNetwork)
            Debug.Log("замена сети на существующую");
        else
        {
            Debug.Log(" дом сменил сеть на " + newNetwork.NetworkNum);
            _currentNetwork = newNetwork;
        }
    }
}
