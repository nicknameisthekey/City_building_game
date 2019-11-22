using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityDebug : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (GameUtility.GetTileUnderMousePosition(out Tile tile))
            {
                if (tile is TileWithBuilding)
                {
                    TileWithBuilding tileWithBuilding = (TileWithBuilding)tile;
                    Debug.Log("В тайле по адресу " + tile.TileID);
                    foreach (var res in tileWithBuilding.Building.currentNetwork.Recources)
                    {
                        Debug.Log(res.Key.ToString() + " " + res.Value);
                    }
                }
                else
                {
                    Debug.Log("Тайл " + tile.TileID + " без строения");
                }
            }
        }

    }
}
