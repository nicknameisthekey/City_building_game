using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBuildingsOnStart : MonoBehaviour
{
    [SerializeField] List<BuildingWithCoords> buildingsToAdd;
    private void Awake()
    {
        if (MapGenerator.MapCreated)
            ConstructOnStart();
        else
            MapGenerator.MapGenerated += ConstructOnStart;
    }
    public void ConstructOnStart()
    {
        Debug.Log("asdasd");
        foreach (var building in buildingsToAdd)
        {
            if (building.BuildingPrefab != null)
                BuildingPlacer.PlaceInstantly(building.BuildingPrefab, building.TileIDToPlace);
        }
    }
}
