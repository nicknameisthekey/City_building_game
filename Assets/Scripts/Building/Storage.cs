using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public Dictionary<RecourceType, float> Recources { get; private set; } = new Dictionary<RecourceType, float>();
    public event Action<RecourceType, float, Storage> RecourceChanged = delegate { };

    public List<RecourceType> AcceptableTypes { get; private set; }
    public float Capacity { get; private set; } = 0;
    public float TotalAmountOfGoods { get; private set; } = 0;

    public Storage() { }
    public Storage(List<RecourceType> acceptableTypes, float capacity)
    {
        AcceptableTypes = acceptableTypes;
        Capacity = capacity;
    }

    public void ChangeRecourceAmount(RecourceType type, float amount)
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
    }
    public bool AddRecource(RecourceType type, float amount)
    {
        if (AcceptableTypes.Contains(type) || AcceptableTypes.Contains(RecourceType.all))
        {
            if (TotalAmountOfGoods + amount <= Capacity)
            {
                ChangeRecourceAmount(type, amount);
                return true;
            }
        }
        return false;
    }
}
