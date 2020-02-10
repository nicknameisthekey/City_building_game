using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalRecources
{
    static Dictionary<GlobalRecourceType, int> recources = new Dictionary<GlobalRecourceType, int>();
    public static event Action<GlobalRecourceType> RecourceChanged = delegate { };
    public static event Action RecourcesChanged = delegate { };
    public static void Initializtion(List<StaticRecource> addOnStart)
    {
        foreach (var item in Enum.GetValues(typeof(GlobalRecourceType)))
        {
            GlobalRecourceType type = (GlobalRecourceType)item;
            recources.Add(type, 0);
        }
        foreach (var res in addOnStart)
        {
            recources[res.Type] += res.Amount;
            RecourcesChanged.Invoke();
            RecourceChanged.Invoke(res.Type);
        }
    }
    public static int GetAmount(GlobalRecourceType type)
    {
        return recources[type];
    }
    public static bool CanChangeAmount(GlobalRecourceType type, int amount)
    {
        if (recources[type] - amount < 0)
            return false;

        recources[type] += amount;
        RecourceChanged.Invoke(type);
        RecourcesChanged.Invoke();
        return true;
    }
    public static bool CanChangeAmount(Dictionary<GlobalRecourceType, int> recourcesToCheck, bool substract)
    {
        if (substract)
        {
            foreach (var kvp in recourcesToCheck)
            {
                if (recources[kvp.Key] - kvp.Value < 0)
                    return false;
            }
        }
        return true;
    }
    public static void SubstractRecources(Dictionary<GlobalRecourceType, int> recourcesToSubstract)
    {
        foreach (var kvp in recourcesToSubstract)
        {
            recources[kvp.Key] -= kvp.Value;
            RecourceChanged.Invoke(kvp.Key);
        }

        RecourcesChanged.Invoke();
    }
    public static void AddRecources(Dictionary<GlobalRecourceType, int> recourcesToAdd)
    {
        foreach (var kvp in recourcesToAdd)
        {
            recources[kvp.Key] += kvp.Value;
            RecourceChanged.Invoke(kvp.Key);
        }
        RecourcesChanged.Invoke();
    }
}
