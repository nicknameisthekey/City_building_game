﻿using System.Collections;
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
    #endregion

}