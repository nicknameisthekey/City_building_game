using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildingParams : ScriptableObject
{
    [SerializeField] List<Recource> constructRecources;
    public List<Recource> ConstructRecources { get => constructRecources; }

}
