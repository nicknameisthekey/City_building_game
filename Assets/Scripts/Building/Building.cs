using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] protected Vector2Int _size;
    protected Vector2Int _tileID;
    public Vector2Int TileID { get => _tileID; }
    public Vector2Int Size { get => _size; }
    public RoadNetwork currentNetwork { get; protected set; }
    public virtual void Initialize(Vector2Int tileID) { _tileID = tileID; }
    public virtual void ChangeStorage(RoadNetwork newStorage)
    {
        if (currentNetwork != newStorage)
        {
            currentNetwork = newStorage;
        }
    }
}
