using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    protected Vector2Int _tileID;
    public Vector2Int TileID { get => _tileID; }
    public virtual void Initialize(Vector2Int tileID) { _tileID = tileID; }
    [SerializeField] Sprite[] sprites;
    public void ShowSprite(Direction direction)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)direction];
    }
}
