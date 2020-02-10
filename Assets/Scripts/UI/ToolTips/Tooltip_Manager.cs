using System;
using UnityEngine;

public class Tooltip_Manager : MonoBehaviour
{
    [SerializeField] GameObject pb_BuildingMenuTooltip_prefab;
    GameObject pb_BuildingMenuTooltip_GO;
    PB_BuildingMenu_Tooltip pb_BuildingMenu_Tooltip;
    public static event Action HideAllTooltips = delegate { };

    static Tooltip_Manager instance;
    void Awake()
    {
        instance = this;
        instatiatePrefabs();
    }

    void instatiatePrefabs()
    {
        pb_BuildingMenuTooltip_GO = Instantiate(pb_BuildingMenuTooltip_prefab, transform);
        pb_BuildingMenu_Tooltip = pb_BuildingMenuTooltip_GO.GetComponentInChildren<PB_BuildingMenu_Tooltip>();
    }
    public static void HideToolTip()
    {
        HideAllTooltips.Invoke();
    }
    public static void ShowTooltip(object model)
    {
        if (model is PB_BuildingMenu_TooltipModel data)
        {
            instance.pb_BuildingMenu_Tooltip.Show(data);
        }
    }
}
