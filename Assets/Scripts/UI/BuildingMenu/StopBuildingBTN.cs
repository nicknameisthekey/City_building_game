using UnityEngine;

public class StopBuildingBTN : MonoBehaviour
{

    private void Awake()
    {
        BuildingPlacer.StartedConstructing += showBTN;
        BuildingPlacer.StopedConstructing += hideBTN;
    }
    void hideBTN()
    {
        gameObject.SetActive(false);
    }
    void showBTN()
    {
        gameObject.SetActive(true);
    }
    public void OnClick()
    {
        BuildingPlacer.StopContructing();
    }
}
