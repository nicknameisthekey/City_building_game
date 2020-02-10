using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_BuildingMenu_TooltipModel //: TooltipData
{
    public string AllDataInText;
    public PB_BuildingMenu_TooltipModel(PassiveBuildingParams pbParams)
    {
        AllDataInText = $"{pbParams.BuildingName}\n" +
            $"Затраты на строительство:\n";
        foreach (var res in pbParams.ConstructRecources)
        {
            AllDataInText += $"{res.Type.ToString()} {res.Amount.ToString()}\n";
        }
        if (pbParams.recourcesRequired)
        {
            AllDataInText += $"Требует для работы:\n";
            foreach (var pres in pbParams.StaticRecourcesRequired)
                AllDataInText += $"{pres.Key.ToString()} {pres.Value.ToString()}\n";
        }
        AllDataInText += $"Дает ресурсы:\n";
        foreach (var pres in pbParams.StaticRecourcesProvided)
            AllDataInText += $"{pres.Key.ToString()} {pres.Value.ToString()}\n";
    }
}
