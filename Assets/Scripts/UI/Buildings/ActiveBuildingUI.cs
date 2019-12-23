using UnityEngine;

public class ActiveBuildingUI : MonoBehaviour
{
    [SerializeField] Ab_Construction_UI constructionUI;
    [SerializeField] AB_Production_UI production_UI;
    static ActiveBuildingUI instance;
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
        constructionUI.Initialize();
        production_UI.Initialize();
        UtilityDebug.CloseAllWindows += closeAllWindows;
    }
    void closeAllWindows()
    {
        instance.constructionUI.Close();
        instance.production_UI.Close();
    }

    public static void ShowUI(ActiveBuildingNew activeBuilding)
    {
        switch (activeBuilding.CurrentState)
        {
            case AB_State_CollectingMaterials state:
                {
                    instance.constructionUI.Show(state);
                    break;
                }
            case AB_State_ProductionCycle state:
                {
                    instance.production_UI.Show(state);
                    break;
                }

        }
        instance.gameObject.SetActive(true);
    }

}
