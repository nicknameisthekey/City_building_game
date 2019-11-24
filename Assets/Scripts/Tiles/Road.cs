using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building, INetwork
{
    Vector2Int[] nearbyIDs = new Vector2Int[4];

    RoadNetwork _currentNetwork;
    public RoadNetwork CurrentNetwork => _currentNetwork;

    public override void Initialize(Vector2Int tileId)
    {
        base.Initialize(tileId);
        fillNearByTileIds();
        checkNearbyTilesForRoads();
        checkNearbyTilesForActivehouses();
    }
    void fillNearByTileIds()
    {
        nearbyIDs[0] = new Vector2Int(_tileID.x + 1, _tileID.y);
        nearbyIDs[1] = new Vector2Int(_tileID.x, _tileID.y + 1);
        nearbyIDs[2] = new Vector2Int(_tileID.x - 1, _tileID.y);
        nearbyIDs[3] = new Vector2Int(_tileID.x, _tileID.y - 1);
    }
    void checkNearbyTilesForRoads()
    {
        foreach (var id in nearbyIDs)
        {
            var nearbyRoad = checkTileForRoad(MapGenerator.Map[id.x, id.y]);
            if (nearbyRoad != null)
            {
                if (_currentNetwork == null)
                {
                    _currentNetwork = nearbyRoad.CurrentNetwork;
                   _currentNetwork.addBuildingToNetwork(this);
                    Debug.Log("присоеденился к существующей сети");
                }
                else if (_currentNetwork == nearbyRoad.CurrentNetwork)
                {
                    Debug.Log("соединился с текущей сетью");
                }
                else if (_currentNetwork != nearbyRoad.CurrentNetwork)
                {
                    Debug.Log("делаю слияние двух дорожных сетей");
                    RoadNetwork.MergeNetworks(_currentNetwork, nearbyRoad.CurrentNetwork);
                }
            }
        }
        if (_currentNetwork == null)
        {
            Debug.Log("создал новую сеть");
            _currentNetwork = new RoadNetwork();
            _currentNetwork.addBuildingToNetwork(this);
        }
    }
    void checkNearbyTilesForActivehouses()
    {
        foreach (var id in nearbyIDs)
        {
            ActiveBuilding building = checkTileForActiveBuilding(MapGenerator.Map[id.x, id.y]);
            //после удаления дороги проверка новой на стоящие дома
            if (building != null && building.TileID + building.RoadConnectionPoint == TileID)
            {
                Debug.Log("нашел дом и должен присоединить его к сети, доделай");
            }
        }
    }
    Road checkTileForRoad(Tile tileToCheck)
    {
        if (tileToCheck is TileWithBuilding)
        {
            TileWithBuilding tileWithBuilding = (TileWithBuilding)tileToCheck;
            if (tileWithBuilding.Building is Road)
            {
                Road road = (Road)tileWithBuilding.Building;
                return road;
            }
        }
        return null;
    }
    ActiveBuilding checkTileForActiveBuilding(Tile tileToCheck)
    {
        if (tileToCheck is TileWithBuilding)
        {
            TileWithBuilding tileWithBuilding = (TileWithBuilding)tileToCheck;
            if (tileWithBuilding.Building is ActiveBuilding)
            {
                ActiveBuilding building = (ActiveBuilding)tileWithBuilding.Building;
                return building;
            }
        }
        return null;
    }

    public void ChangeRoadNetwork(RoadNetwork newNetwork)
    {
        if (_currentNetwork == newNetwork)
            Debug.Log("замена сети на существующую");
        else _currentNetwork = newNetwork;
    }
}
