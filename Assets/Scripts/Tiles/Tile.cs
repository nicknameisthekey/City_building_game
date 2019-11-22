using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    Vector2Int _tileID;
    public Vector2Int TileID { get => _tileID; }
    public GameObject TileGo { get; }
    public Tile(GameObject GO, Vector2Int ID)
    {
        TileGo = GO;
        _tileID = ID;
    }
}
