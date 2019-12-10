using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public static event Action<StorageBuilding> NewStorageBuildingPlaced = delegate { };
    public static Node[,] RoadMap { get; private set; }
    public static List<StorageBuilding> StorageBuildings = new List<StorageBuilding>();
    public static void Initialize(int sideSize)
    {
        RoadMap = new Node[sideSize, sideSize];
        for (int x = 0; x < MapGenerator.SideSize; x++)
            for (int y = 0; y < MapGenerator.SideSize; y++)
                RoadMap[x, y] = new Node(false, Vector3.zero, x, y);
    }
    public static void PlaceBuildingOnMap(GameObject GO, Building building, Vector2Int tileID)
    {
        GameObject.Destroy(MapGenerator.TileMap[tileID.x, tileID.y].TileGo);
        MapGenerator.TileMap[tileID.x, tileID.y] = new TileWithBuilding(GO, tileID, building);
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
