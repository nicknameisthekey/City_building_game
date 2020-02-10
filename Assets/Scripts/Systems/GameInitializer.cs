using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameSettings settings;
    [SerializeField] UtilityDebug utilityDebug;
    [SerializeField] MapGenerator mapGen;
    [SerializeField] CameraMovement CameraMovement;
    [SerializeField] UIManager UIManager;
    private void Start()
    {
        utilityDebug.Initialization(settings);
        Tile[,] tileMap = mapGen.GenerateMap(settings);
        Map.Initialize(settings, tileMap);
        GlobalRecources.Initializtion(settings.StaringStaticRes);
        constructOnGameStart();
        CameraMovement.Initialize();
        if (Map.StorageBuildings.Count != 0)
        {
            foreach (var res in settings.Startingrecource)
                Map.StorageBuildings[0].Storage.AddMaximumAmount(res.Type, res.Amount, out int c);
        }
        UIManager.Initialize();

    }

    private void constructOnGameStart()
    {
        foreach (var building in settings.BuildingsToAdd)
        {
            if (building.BuildingPrefab != null)
                BuildingPlacer.PlaceInstantly(building.BuildingPrefab, building.TileIDToPlace, building.Direction);
        }
    }
}
