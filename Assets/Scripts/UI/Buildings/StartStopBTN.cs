using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopBTN : MonoBehaviour
{
    ActiveBuildingNew currentBuilding;
    public void Initialize(ActiveBuildingNew building)
    {
        currentBuilding = building;
    }
    public void OnClick()
    {
        currentBuilding.StartStopWork();
    }
}
