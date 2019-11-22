using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building
{
    Vector2Int[] nearbyIDs = new Vector2Int[4];
    List<Vector2Int> nearbyHouses = new List<Vector2Int>();
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
                if (currentNetwork == null)
                {
                    currentNetwork = nearbyRoad.currentNetwork;
                    currentNetwork.addBuildingToNetwork(this);
                    Debug.Log("присоеденился к существующей сети");
                }
                else if (currentNetwork == nearbyRoad.currentNetwork)
                {
                    //делать ничего не нужно?
                }
                else if (currentNetwork != nearbyRoad.currentNetwork)
                {
                    Debug.Log("делаю слияние двух цепей");
                    RoadNetwork.MergeStorages(currentNetwork, nearbyRoad.currentNetwork);
                }
            }
        }
        if (currentNetwork == null)
        {
            currentNetwork = new RoadNetwork();
            currentNetwork.addBuildingToNetwork(this);
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
                Debug.Log("этот дом стоит правильно");
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

}
