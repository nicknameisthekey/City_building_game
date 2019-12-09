using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static bool MapCreated { get; private set; }
    [SerializeField] int _sideSize;
    [SerializeField] GameObject tile;
    [SerializeField] Sprite greenTile;
    [SerializeField] Sprite treeTile;
    [SerializeField] float perlinNoiseSeed;
    [SerializeField] Transform transformToAttachTiles;
    public static event Action MapGenerated = delegate { };

    public static int SideSize { get; private set; }
    public static Tile[,] TileMap { get; private set; }
    float MapOffsetX;
    float MapOffsetY;
    private void Awake()
    {
        MapOffsetX = _sideSize * 0.5f;
        MapOffsetY = _sideSize * 0.25f;
        SideSize = _sideSize;
        TileMap = new Tile[_sideSize, _sideSize];
        Map.Initialize(_sideSize);
        generateTileMap();
    }
    void generateTileMap()
    {
        for (int x = 0; x < _sideSize; x++)
        {
            for (int y = 0; y < _sideSize; y++)
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
        MapGenerated.Invoke();
        MapCreated = true;
    }
    void placeTile(int x, int y, Sprite sprite)
    {
        GameObject GO;
        float realY;
        float realX;
        realX = (x - y) * 0.5f + MapOffsetX;
        realY = (x + y) * 0.25f + MapOffsetY;

        GO = Instantiate(tile, new Vector3(realX, realY, 0), Quaternion.identity, transformToAttachTiles);
        GO.GetComponent<SpriteRenderer>().sprite = sprite;
        TileMap[x, y] = new Tile(GO, new Vector2Int(x, y));
        GO.name = "Tile " + x + ":" + y;
    }
}

