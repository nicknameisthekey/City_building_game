using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStopBTN : MonoBehaviour
{
    ActiveBuilding currentBuilding;
    public void Initialize(ActiveBuilding building)
    {
        currentBuilding = building;
    }
    public void OnClick()
    {
        currentBuilding.StartStopWork();
    }
}
