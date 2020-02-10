using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject buildingPrefab;
    public void OnClick()
    {
        BuildingPlacer.StartPlacing(buildingPrefab);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ITooltipConstruction tooltipConstruction = buildingPrefab.GetComponent<ITooltipConstruction>();
        if (tooltipConstruction == null)
        {
            Debug.Log("interface null");
            return;
        }
        Tooltip_Manager.ShowTooltip(tooltipConstruction.GetTooltipModel());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip_Manager.HideToolTip();
    }
}
