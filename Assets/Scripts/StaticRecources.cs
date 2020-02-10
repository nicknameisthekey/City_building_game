using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticRecources
{
    static Dictionary<StaticRecourceType, int> recources = new Dictionary<StaticRecourceType, int>();
    public static event Action<StaticRecourceType> RecourceChanged = delegate { };
    public static event Action RecourcesChanged = delegate { };
    public static void Initializtion(List<StaticRecource> addOnStart)
    {
        foreach (var item in Enum.GetValues(typeof(StaticRecourceType)))
        {
            StaticRecourceType type = (StaticRecourceType)item;
            recources.Add(type, 0);
        }
        foreach (var res in addOnStart)
        {
            recources[res.Type] += res.Amount;
            RecourcesChanged.Invoke();
            RecourceChanged.Invoke(res.Type);
        }
    }
    public static int GetAmount(StaticRecourceType type)
    {
        return recources[type];
    }
    public static bool CanChangeAmount(StaticRecourceType type, int amount)
    {
        if (recources[type] - amount < 0)
            return false;

        recources[type] += amount;
        RecourceChanged.Invoke(type);
        RecourcesChanged.Invoke();
        return true;
    }
    public static bool CanChangeAmount(Dictionary<StaticRecourceType, int> recourcesToCheck, bool substract)
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
    public static void SubstractRecources(Dictionary<StaticRecourceType, int> recourcesToSubstract)
    {
        foreach (var kvp in recourcesToSubstract)
        {
            recources[kvp.Key] -= kvp.Value;
            RecourceChanged.Invoke(kvp.Key);
        }

        RecourcesChanged.Invoke();
    }
    public static void AddRecources(Dictionary<StaticRecourceType, int> recourcesToAdd)
    {
        foreach (var kvp in recourcesToAdd)
        {
            recources[kvp.Key] += kvp.Value;
            RecourceChanged.Invoke(kvp.Key);
        }
        RecourcesChanged.Invoke();
    }
}
