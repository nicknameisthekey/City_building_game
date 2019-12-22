using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticRecources
{
    static Dictionary<StaticRecourceType, int> recources = new Dictionary<StaticRecourceType, int>();
    public static event Action<StaticRecourceType> RecourceChanged = delegate { };
    public static event Action RecourcesChanged = delegate { };
    public static void Initializtion(Dictionary<StaticRecourceType, int> addOnStart)
    {
        foreach (var item in Enum.GetValues(typeof(StaticRecourceType)))
        {
            StaticRecourceType type = (StaticRecourceType)item;
            recources.Add(type, 0);
        }
        foreach (var kvp in addOnStart)
        {
            recources[kvp.Key] += kvp.Value;
            RecourcesChanged.Invoke();
            RecourceChanged.Invoke(kvp.Key);
        }
    }
    public static int GetAmount(StaticRecourceType type)
    {
        return recources[type];
    }
    public static bool ChangeAmount(StaticRecourceType type, int amount)
    {
        if (recources[type] - amount < 0)
            return false;

        recources[type] += amount;
        RecourceChanged.Invoke(type);
        RecourcesChanged.Invoke();
        return true;
    }
    public static bool ChangeAmount(Dictionary<StaticRecourceType, int> recourcesToCheck)
    {
        
        foreach (var kvp in recourcesToCheck)
        {
            if (recources[kvp.Key] + kvp.Value < 0)
                return false;
        }
        foreach (var kvp in recourcesToCheck)
        {
            Debug.Log(kvp.Key + " " +kvp.Value);
            recources[kvp.Key] += kvp.Value;
            RecourceChanged.Invoke(kvp.Key);
            RecourcesChanged.Invoke();
        }
        return true;
    }
}
