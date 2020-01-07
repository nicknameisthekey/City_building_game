using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameSettings settings;
    [SerializeField] MapGenerator mapGen;
    [SerializeField] CameraMovement CameraMovement;
    [SerializeField] UIManager UIManager;
    private void Start()
    {
        Tile[,] tileMap = mapGen.GenerateMap(settings);
        Map.Initialize(settings, tileMap);
        StaticRecources.Initializtion(settings.StaringStaticRes);
        constructOnGameStart();
        CameraMovement.Initialize();
        if (Map.StorageBuildings.Count != 0)
        {
            foreach(var res in settings.Startingrecource)
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
