using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Sprite greenTile;
    [SerializeField] Sprite treeTile;

    [SerializeField] Transform transformToAttachTiles;

    int mapSideSize;
    float perlinNoiseSeed;
    float mapOffsetX;
    float mapOffsetY;
    public Tile[,] tileMap;
    
    public Tile[,] GenerateMap(GameSettings settings)
    {
        mapSideSize = settings.MapSideSize;
        perlinNoiseSeed = settings.PerlinNoiseSeed;
        mapOffsetX = mapSideSize * 0.5f;
        mapOffsetY = mapSideSize * 0.25f;
        tileMap = new Tile[mapSideSize, mapSideSize];
        return generateTileMap();
    }
    public Tile[,] generateTileMap()
    {
        Debug.Log(mapSideSize);
        for (int x = 0; x < mapSideSize; x++)
        {
            for (int y = 0; y < mapSideSize; y++)
            {
                float noise = Mathf.PerlinNoise(x * perlinNoiseSeed, y * perlinNoiseSeed);
                //tree
                if (noise > 0.5)
                {
                    placeTile(x, y, treeTile);
                }
                //empty
                else
                    placeTile(x, y, greenTile);
            }
        }
        return tileMap;
    }
    void placeTile(int x, int y, Sprite sprite)
    {
        GameObject GO;
        float realY = (x - y) * 0.5f + mapOffsetX; 
        float realX = (x + y) * 0.25f + mapOffsetY; 

        GO = Instantiate(tilePrefab, new Vector3(realX, realY, 0), Quaternion.identity, transformToAttachTiles);
        GO.GetComponent<SpriteRenderer>().sprite = sprite;
        GO.name = "Tile " + x + ":" + y;
        tileMap[x, y] = new Tile(GO, new Vector2Int(x, y));
    }
}

