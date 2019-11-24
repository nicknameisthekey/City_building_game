
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] Sprite constructionPlaceHighlight;

    public static event Action StartedConstructing = delegate { };
    public static event Action StopedConstructing = delegate { };
    static GameObject currentGO;
    static BuildingPlacer instance;
    static Vector3 mousePos;
    private void Awake()
    {
        instance = this;
    }
    public static void StartPlacing(GameObject prefab)
    {
        StartedConstructing.Invoke();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentGO = Instantiate(prefab, new Vector2(mousePos.x, mousePos.y), Quaternion.identity);
        Building building = currentGO.GetComponent<Building>();
        instance.StartCoroutine(instance.draggingCour(building));
    }
    IEnumerator draggingCour(Building building)
    {
        while (true)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tileID = GameUtility.GetTileIDUnderMousePosition();
            if (GameUtility.CheckIDIfValid(tileID))
                currentGO.transform.position = MapGenerator.Map[tileID.x, tileID.y].TileGo.transform.position + getOffsetForBuilding(building.Size);
            else
                currentGO.transform.position = new Vector2(mousePos.x, mousePos.y);
            bool canPlace = canPlaceHere(building, tileID);
            if (canPlace)
            {
                // Debug.Log("can");
            }
            //  else Debug.Log("cannot");


            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && canPlace)
            {
                if (tryPlace(building, tileID))
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    Vector3 getOffsetForBuilding(Vector2 size)
    {
        Vector3 offset = Vector3.zero;
        if (size.x == 1 & size.y == 2)
        {
            offset.x = 0.25f;
            offset.y = 0.15f;
        }
        return offset;
    }

    bool canPlaceHere(Building building, Vector2Int currentPos)
    {
        if (GameUtility.CheckIDIfValid(currentPos) && MapGenerator.Map[currentPos.x, currentPos.y] is TileWithBuilding)
        {
            //Debug.Log("tile with building or current pos was not valid");
            return false;
        }
        else if (building is ActiveBuilding)
        {
            return canPlaceActiveBuilding((ActiveBuilding)building, currentPos);
        }
        else if (building is StorageBuilding)
        {
            return canPlaceStorageBuilding((StorageBuilding)building, currentPos);
        }
        return true;
    }
    bool canPlaceStorageBuilding(StorageBuilding building, Vector2Int currentPos)
    {
        StorageBuilding storageBuilding = (StorageBuilding)building;
        Vector2Int roadPointID = currentPos + storageBuilding.RoadConnectionPoint;
        if (!GameUtility.CheckIDIfValid(roadPointID))
        {
            Debug.Log("координаты точки дороги за границами массива " + roadPointID);
            Debug.Log("Mouse at " + currentPos + " road must be on " + roadPointID);
            return false;
        }

        //  Debug.Log("Mouse at " + currentPos + " road must be on " + roadPointID);
        Tile possibleRoadTile = MapGenerator.Map[roadPointID.x, roadPointID.y];
        if (possibleRoadTile is TileWithBuilding)
        {
            TileWithBuilding TWB = (TileWithBuilding)possibleRoadTile;
            if (TWB.Building is Road)
                return true;
            else
                return false;
        }
        else return false;
    }
    bool canPlaceActiveBuilding(ActiveBuilding building, Vector2Int currentPos)
    {
        ActiveBuilding active = (ActiveBuilding)building;
        Vector2Int roadPointID = currentPos + active.RoadConnectionPoint;
        if (!GameUtility.CheckIDIfValid(roadPointID))
        {
            Debug.Log("координаты точки дороги за границами массива " + roadPointID);
            Debug.Log("Mouse at " + currentPos + " road must be on " + roadPointID);
            return false;
        }

        //  Debug.Log("Mouse at " + currentPos + " road must be on " + roadPointID);
        Tile possibleRoadTile = MapGenerator.Map[roadPointID.x, roadPointID.y];
        if (possibleRoadTile is TileWithBuilding)
        {
            TileWithBuilding TWB = (TileWithBuilding)possibleRoadTile;
            if (TWB.Building is Road)
                return true;
            else
                return false;
        }
        else return false;
    }
    public static void StopContructing()
    {
        instance.StopAllCoroutines();
        GameObject.Destroy(currentGO);
        StopedConstructing.Invoke();
    }
    bool tryPlace(Building building, Vector2Int tileID)
    {

        GameObject.Destroy(MapGenerator.Map[tileID.x, tileID.y].TileGo);
        building.Initialize(tileID);
        MapGenerator.Map[tileID.x, tileID.y] = new TileWithBuilding(currentGO, tileID, building);
        StopedConstructing.Invoke();
        return true;
    }
}
