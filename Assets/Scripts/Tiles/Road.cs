using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : StandaloneBuilding
{
    public override void changeState(BuildingState newstate)
    {
        throw new System.NotImplementedException();
    }

    public override void finishConstruction()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize(Vector2Int tileId)
    {
        base.Initialize(tileId);
    }
}
