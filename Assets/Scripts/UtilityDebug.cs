using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UtilityDebug : MonoBehaviour
{
    [SerializeField] GameSettings settings;
    public static bool ActivebuildingLog { get; private set; }
    public static bool PassivebuildingLog { get; private set; }
    public static bool BuildingConstructionLog { get; private set; }
    public void Initialization(GameSettings settings)
    {
        ActivebuildingLog = settings.ActiveBuildingLog;
        PassivebuildingLog = settings.PassiveBuildingLog;
        BuildingConstructionLog = settings.BuildingConstructionLog;
    }

    Vector2Int first;
    void Update()
    {
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
                        TileWithBuilding t = (TileWithBuilding)Map.TileMap[n.gridX, n.gridY];
                        t.TileGo.SetActive(false);
                        // Debug.Log(n.gridX + "," + n.gridY);
                    }
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            TileWithBuilding t = (TileWithBuilding)GameUtility.GetTileUnderMousePosition();
            BuildingNearRoad b = (BuildingNearRoad)t.Building;
            Debug.Log(GameUtility.GetTileIDUnderMousePosition() + " " + b.RoadIDItConnects);
        }

    }
}
