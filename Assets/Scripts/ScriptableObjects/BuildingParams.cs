using System.Collections.Generic;
using UnityEngine;
public class BuildingParams : ScriptableObject
{
    [SerializeField] List<Recource> constructRecources;
    [SerializeField] string buildingName;
    public List<Recource> ConstructRecources { get => constructRecources; }
    public string BuildingName { get => buildingName == null ? "" : buildingName; }
}
