using UnityEngine;

public class BuildingInfoUI : MonoBehaviour
{
    [SerializeField] Construction_UI constructionUI;
    [SerializeField] AB_Production_UI ab_production_UI;
    [SerializeField] PB_ProductionUI pb_Production_UI;
    static BuildingInfoUI instance;
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
        constructionUI.Initialize();
        ab_production_UI.Initialize();
        pb_Production_UI.Initialize();
        InputHandler.CloseAllWindowsPressed += closeAllWindows;
    }
    void closeAllWindows()
    {
        instance.constructionUI.Close();
        instance.ab_production_UI.Close();
        instance.pb_Production_UI.Close();
    }

    public static void ShowUI(BuildingNearRoad Building)
    {
        switch (Building.CurrentState)
        {
            case State_Construction state:
                {
                    instance.constructionUI.Show(state);
                    break;
                }
            case AB_State_ProductionCycle state:
                {
                    instance.ab_production_UI.Show(state);
                    break;
                }
            case PB_ProductionCycle state:
                {
                    instance.pb_Production_UI.Show(state);
                    break;
                }
            default: break;
        }
        instance.gameObject.SetActive(true);
    }

}
