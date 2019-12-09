using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UtilityDebug : MonoBehaviour
{
    Vector2Int first;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (GameUtility.GetTileUnderMousePosition(out Tile tile))
            {
                if (tile is TileWithBuilding)
                {
                    TileWithBuilding tileWithBuilding = (TileWithBuilding)tile;
                    Debug.Log("В тайле по адресу " + tile.TileID);
                    if (tileWithBuilding.Building is StorageBuilding)
                    {
                        StorageBuilding storageBuilding = (StorageBuilding)tileWithBuilding.Building;
                        StorageInfo.ShowRecources(storageBuilding.Storage);
                    }
                }
                else
                {
                    Debug.Log("Тайл " + tile.TileID + " без строения");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            StorageInfo.HideRecourceInfo();
        }
        if (Input.GetKeyDown(KeyCode.J) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (first == Vector2Int.zero)
            {
                first = GameUtility.GetTileIDUnderMousePosition();
                Debug.Log("добавил первый " + first);
            }
            else
            {
                Debug.Log(GameUtility.GetTileIDUnderMousePosition());
                var path = Pathfinding.FindPath(first, GameUtility.GetTileIDUnderMousePosition());
                if (path != null)
                    foreach (var n in path)
                    {
                        TileWithBuilding t = (TileWithBuilding)MapGenerator.TileMap[n.gridX, n.gridY];
                        t.TileGo.SetActive(false);
                        // Debug.Log(n.gridX + "," + n.gridY);
                    }
            }
        }

    }
}
