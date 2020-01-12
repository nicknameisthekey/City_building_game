using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField] string buildingName;
    [SerializeField] Sprite[] sprites;

    static int buildingID = 0;

    public BuildingParams BuildingParams { get; protected set; }
    public Vector2Int TileID { get; protected set; }
    public string BuildingName { get; protected set; }

    public event Action<BuildingState> StateChanged = delegate { };
    public BuildingState CurrentState { get; protected set; }
    public virtual void Initialize(Vector2Int tileID) { TileID = tileID; buildingID++; BuildingName = $"[{buildingID}] [{buildingName}]"; }
    public void ShowSprite(Direction direction) => gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)direction];
    protected void stateChangedInvoke(BuildingState newState) => StateChanged.Invoke(newState);
    public abstract void changeState(BuildingState newstate);
    public abstract void finishConstruction();
}
