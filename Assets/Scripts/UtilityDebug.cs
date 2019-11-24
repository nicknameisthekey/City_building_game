using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UtilityDebug : MonoBehaviour
{

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

    }
}
