using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{
    [SerializeField] GameObject buildingPrefab;
    public void OnClick()
    {
        BuildingPlacer.StartPlacing(buildingPrefab);
    }
}
