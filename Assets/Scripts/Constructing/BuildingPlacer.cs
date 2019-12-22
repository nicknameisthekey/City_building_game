
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    public static event Action StartedConstructing = delegate { };
    public static event Action StopedConstructing = delegate { };

    static Direction currentRotation = Direction.BR;

    static GameObject currentGO;
    static Building currentBuilding;
    static BuildingPlacer instance;
    static Vector3 mousePos;
    private void Awake()
    {
        instance = this;
        StopedConstructing += onStopConstruction;
    }
    public static void StartPlacing(GameObject prefab)
    {
        StartedConstructing.Invoke();
        InputHandler.RotationKeyPressed += rotate;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentGO = Instantiate(prefab, new Vector2(mousePos.x, mousePos.y), Quaternion.identity);
        currentBuilding = currentGO.GetComponent<Building>();
        currentBuilding.ShowSprite(currentRotation);
        instance.StartCoroutine(instance.draggingCour(currentBuilding));
    }
    static void rotate()
    {
        currentRotation++;
        if ((int)currentRotation > 3)
            currentRotation = 0;
        currentBuilding.ShowSprite(currentRotation);
    }
    IEnumerator draggingCour(Building building)
    {
        while (true)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool canPlace;
            if (GameUtility.GetTileIDUnderMousePosition(out Vector2Int tileID))
            {
                currentGO.transform.position = Map.TileMap[tileID.x, tileID.y].TileGo.transform.position;
                canPlace = canPlaceHere(building, tileID);
            }
            else
            {
                currentGO.transform.position = new Vector2(mousePos.x, mousePos.y);
                canPlace = false;
            }
            if (canPlace)
            {
                // Debug.Log("can");
            }
            //  else Debug.Log("cannot");
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && canPlace)
            {
                place(tileID);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    bool canPlaceHere(Building building, Vector2Int currentPos)
    {
        if (Map.TileMap[currentPos.x, currentPos.y] is TileWithBuilding)
        {
            //Debug.Log("tile with building or current pos was not valid");
            return false;
        }
        else if (building is BuildingNearRoad)
        {
            Vector2Int roadPointID = currentPos + GameUtility.GetNearbyIDByDirection(currentRotation);
            if (GameUtility.GetRoadByID(roadPointID, out Road road))
                return true;
        }
        else if (building is StandaloneBuilding)
            return true;
        return false;
    }

    public static void StopContructing()
    {
        instance.StopAllCoroutines();
        GameObject.Destroy(currentGO);
        StopedConstructing.Invoke();
    }
    static void place(Vector2Int tileID)
    {
        if (currentBuilding is StandaloneBuilding)
        {
            currentBuilding.Initialize(tileID);
        }
        else if (currentBuilding is BuildingNearRoad)
        {
            BuildingNearRoad bnr = (BuildingNearRoad)currentBuilding;
            bnr.Initialize(tileID, tileID + GameUtility.GetNearbyIDByDirection(currentRotation));
        }
        Map.PlaceBuildingOnMap(currentGO, currentBuilding, tileID);
        currentBuilding = null;
        StopedConstructing.Invoke();
    }
    public static void PlaceInstantly(GameObject PrefabToPlace, Vector2Int tileID)
    {
        currentGO = Instantiate(PrefabToPlace, Map.TileMap[tileID.x, tileID.y].TileGo.transform.position, Quaternion.identity);
        currentBuilding = currentGO.GetComponent<Building>();
        place(tileID);
    }
    static void onStopConstruction()
    {
        InputHandler.RotationKeyPressed -= rotate;
    }
}
