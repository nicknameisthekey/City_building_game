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
                    NetworkInfo.ShowRecourcesOnNetwork(tileWithBuilding.Building.currentNetwork);
                }
                else
                {
                    Debug.Log("Тайл " + tile.TileID + " без строения");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            NetworkInfo.HideRecourceInfo();
        }

    }
}
