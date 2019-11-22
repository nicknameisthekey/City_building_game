using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWithBuilding : Tile
{
    public Building Building;
    public TileWithBuilding(GameObject GO, Vector2Int ID, Building building) : base(GO, ID) { Building = building; }
}
