using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public Dictionary<RecourceType, int> Recources { get; private set; } = new Dictionary<RecourceType, int>();
    public event Action<RecourceType, int, Storage> RecourceChanged = delegate { };
    public event Action RecourcesChanged = delegate { };

    public List<RecourceType> AcceptableTypes { get; private set; }
    public int Capacity { get; private set; } = 0;
    public int TotalAmountOfGoods { get; private set; } = 0;

    public Storage() { }
    public Storage(List<RecourceType> acceptableTypes, int capacity)
    {
        AcceptableTypes = acceptableTypes;
        Capacity = capacity;
    }

    void changeRecourceAmount(RecourceType type, int amount)
    {
        if (Recources.ContainsKey(type))
        {
            Recources[type] += amount;
        }
        else
        {
            Recources.Add(type, amount);
        }
        TotalAmountOfGoods += amount;
        RecourceChanged.Invoke(type, Recources[type], this);
        RecourcesChanged.Invoke();
    }
    //переделать! дыры в логике!
    public void ChangeRecource(RecourceType type, int amount)
    {
        if (CanChangeRecource(type, amount))
            changeRecourceAmount(type, amount);
        else
            Debug.Log("впихиваю невпихуемое");
    }
    public void ChangeRecources(Dictionary<RecourceType, int> recourcesToAdd)
    {
        if (CanChangeRecources(recourcesToAdd))
            foreach (var kvp in recourcesToAdd)
                changeRecourceAmount(kvp.Key, kvp.Value);
    }
    public bool CanChangeRecource(RecourceType type, int amount)
    {
        if (amount < 0 && Recources.ContainsKey(type) && Recources[type] + amount >= 0)
            return true;
        if (amount >= 0 && (AcceptableTypes.Contains(type) || AcceptableTypes.Contains(RecourceType.all)) && TotalAmountOfGoods + amount <= Capacity)
            return true;
        return false;
    }
    public bool CanChangeRecources(Dictionary<RecourceType, int> recourcesToAdd)
    {
        foreach (var kvp in recourcesToAdd)
        {
            if (!CanChangeRecource(kvp.Key, kvp.Value))
                return false;
        }
        return true;
    }
}
