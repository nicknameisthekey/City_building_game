using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : BuildingNearRoad
{
    [SerializeField] List<RecourceType> _acceptableTypes;
    [SerializeField] int _capacity;

    public Storage Storage { get; private set; }
    public override void Initialize(Vector2Int tileID, Vector2Int roadConnectionPoint)
    {
        base.Initialize(tileID, roadConnectionPoint);
        Storage = new Storage(_acceptableTypes, _capacity);
    }
}
