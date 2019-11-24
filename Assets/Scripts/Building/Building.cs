using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] protected Vector2Int _size;
    protected Vector2Int _tileID;
    public Vector2Int TileID { get => _tileID; }
    public Vector2Int Size { get => _size; }
    
    public virtual void Initialize(Vector2Int tileID) { _tileID = tileID; }
    
}
