using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_BuildingMenu_Tooltip : Tooltip
{
    public void Show(PB_BuildingMenu_TooltipModel model)
    {
        ShowTooltip(model.AllDataInText);
    }
}
