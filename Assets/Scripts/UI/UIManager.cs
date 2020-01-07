﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    [SerializeField] GameObject buildingUIPrefab;
    [SerializeField] GameObject recourceInfoPrefab;
    [SerializeField] GameObject staticRecourcePannelPrefab;
    [SerializeField] GameObject activebuildingUIPrefab;
    Transform UITransform;
    static GameObject _buildingUI;
    static GameObject _recourceInfoPrefab;
    static GameObject _staticRecourcesPanel;
    static GameObject _activeBuildingUI;


    public void Initialize()
    {
        instance = this;
        UITransform = gameObject.transform;
        instantiateUI();
        InputHandler.ShowInfoPressed += showBuildingInfo;
    }

    void instantiateUI()
    {
        _buildingUI = Instantiate(buildingUIPrefab, UITransform);
        _recourceInfoPrefab = Instantiate(recourceInfoPrefab, UITransform);
        _recourceInfoPrefab.SetActive(false);
        _staticRecourcesPanel = Instantiate(staticRecourcePannelPrefab, UITransform);
        _activeBuildingUI = Instantiate(activebuildingUIPrefab, UITransform);
    }
    void showBuildingInfo()
    {
        if (GameUtility.GetTileUnderMousePosition(out Tile tile))
        {
            if (tile is TileWithBuilding)
            {
                TileWithBuilding tileWithBuilding = (TileWithBuilding)tile;
                if (tileWithBuilding.Building is StorageBuilding)
                {
                    StorageBuilding storageBuilding = (StorageBuilding)tileWithBuilding.Building;
                    StorageInfo.ShowRecources(storageBuilding.Storage);
                }
                else if (tileWithBuilding.Building is ActiveBuildingNew)
                {
                    ActiveBuildingNew ab = (ActiveBuildingNew)tileWithBuilding.Building;
                    ActiveBuildingUI.ShowUI(ab);
                }
            }
            else
            {
                Debug.Log("Тайл " + tile.TileID + " без строения");
            }
        }
    }

}
