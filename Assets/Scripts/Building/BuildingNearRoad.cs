using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNearRoad : Building
{
    protected Vector2Int _roadIDItConnects;
    public Vector2Int RoadIDItConnects => _roadIDItConnects;
    protected Road roadItConnects;
    public virtual void Initialize(Vector2Int tileID, Vector2Int roadIDItConnects)
    {
        base.Initialize(tileID);
        _roadIDItConnects = roadIDItConnects;
        roadItConnects = GameUtility.GetRoadByID(roadIDItConnects);
    }
}
