using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNearRoad : Building
{
    protected Vector2Int _roadConnectionPoint;
    public Vector2Int RoadConnectionPoint => _roadConnectionPoint;
    public virtual void Initialize(Vector2Int tileID, Vector2Int roadConntectionPoint)
    {
        base.Initialize(tileID);
        _roadConnectionPoint = roadConntectionPoint;
    }
}
