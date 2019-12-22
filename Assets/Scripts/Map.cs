using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public static event Action<StorageBuilding> NewStorageBuildingPlaced = delegate { };

    public static List<StorageBuilding> StorageBuildings = new List<StorageBuilding>();

    public static Tile[,] TileMap { get; private set; }
    public static Node[,] RoadMap { get; private set; }
    public static int MapSideSize { get; private set; }

    public static void Initialize(GameSettings settings, Tile[,] tileMap)
    {
        MapSideSize = settings.MapSideSize;
        TileMap = tileMap;
        createRoadMapNodes();
    }

    private static void createRoadMapNodes()
    {
        RoadMap = new Node[MapSideSize, MapSideSize];
        for (int x = 0; x < MapSideSize; x++)
            for (int y = 0; y < MapSideSize; y++)
                RoadMap[x, y] = new Node(false, Vector3.zero, x, y);
    }

    public static void PlaceBuildingOnMap(GameObject GO, Building building, Vector2Int tileID)
    {
        GameObject.Destroy(TileMap[tileID.x, tileID.y].TileGo);
        TileMap[tileID.x, tileID.y] = new TileWithBuilding(GO, tileID, building);
        if (building is Road)
        {
            RoadMap[tileID.x, tileID.y] = new Node(true, GO.transform.position, tileID.x, tileID.y);
        }
        else if (building is StorageBuilding)
        {
            StorageBuilding st = (StorageBuilding)building;
            NewStorageBuildingPlaced.Invoke(st);
            StorageBuildings.Add(st);
        }

    }
}
