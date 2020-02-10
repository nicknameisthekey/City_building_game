using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Game Settings", menuName = "Create new Game Settings", order = 51)]
public class GameSettings : ScriptableObject
{
    #region Map_Settings
    [SerializeField] int _sideSize;
    public int MapSideSize { get => _sideSize; }

    [SerializeField] float perlinNoiseSeed;
    public float PerlinNoiseSeed { get => perlinNoiseSeed; }
    #endregion

    #region ToDo_OnGameStart
    [SerializeField] List<BuildingWithCoords> buildingsToAdd;
    public List<BuildingWithCoords> BuildingsToAdd { get => buildingsToAdd; }

    [SerializeField] List<Recource> startingRecources;
    public List<Recource> Startingrecource { get => startingRecources; }
    [SerializeField] List<StaticRecource> startingStaticRes;
    public List<StaticRecource> StaringStaticRes { get => startingStaticRes; }
    #endregion

    #region LogSettings
    [SerializeField] bool activeBuildingLog;
    public bool ActiveBuildingLog { get => activeBuildingLog; }
    [SerializeField] bool passiveBuildingLog;
    public bool PassiveBuildingLog { get => passiveBuildingLog; }
    [SerializeField] bool buildingConstructionLog;
    public bool BuildingConstructionLog { get => buildingConstructionLog; }

    #endregion


}
