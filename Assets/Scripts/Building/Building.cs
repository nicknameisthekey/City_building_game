using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    static int buildingID = 0;
    protected Vector2Int _tileID;
    [SerializeField] string buildingName;
    public string BuildingName { get; protected set; }
    public Vector2Int TileID { get => _tileID; }
    public virtual void Initialize(Vector2Int tileID) { _tileID = tileID; buildingID++; BuildingName = buildingName + " " + buildingID; }
    [SerializeField] Sprite[] sprites;
    public void ShowSprite(Direction direction)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)direction];
    }
}
