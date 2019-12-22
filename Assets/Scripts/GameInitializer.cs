using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameSettings settings;
    [SerializeField] MapGenerator mapGen;
    [SerializeField] CameraMovement CameraMovement;

    private void Start()
    {
        Tile[,] tileMap = mapGen.GenerateMap(settings);
        Map.Initialize(settings, tileMap);
        constructOnGameStart();
        CameraMovement.Initialize();

        /* Dictionary<StaticRecourceType, int> atstart = new Dictionary<StaticRecourceType, int>();
       atstart.Add(StaticRecourceType.people, 10);
       StaticRecources.Initializtion(atstart);
       */
    }

    private void constructOnGameStart()
    {
        foreach (var building in settings.BuildingsToAdd)
        {
            if (building.BuildingPrefab != null)
                BuildingPlacer.PlaceInstantly(building.BuildingPrefab, building.TileIDToPlace);
        }
    }
}
