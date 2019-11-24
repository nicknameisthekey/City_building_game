using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : Building, INetwork
{
    [SerializeField] List<RecourceType> _acceptableTypes;
    [SerializeField] float _capacity;

    [SerializeField] Vector2Int _roadConnectionPoint;
    public Vector2Int RoadConnectionPoint => _roadConnectionPoint;

    RoadNetwork _currentNetwork;
    public RoadNetwork CurrentNetwork => _currentNetwork;

    public Storage Storage { get; private set; }
    public override void Initialize(Vector2Int tileID)
    {
        base.Initialize(tileID);
        Storage = new Storage(_acceptableTypes, _capacity);

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
            }
        }
        else
            Debug.Log("road null witch cant be");

    }

    public void ChangeRoadNetwork(RoadNetwork newNetwork)
    {
        if (_currentNetwork == newNetwork)
            Debug.Log("замена сети на существующую");
        else
        {
            Debug.Log(" склад сменил сеть на " + newNetwork.NetworkNum);
            _currentNetwork = newNetwork;
        }
    }


}
