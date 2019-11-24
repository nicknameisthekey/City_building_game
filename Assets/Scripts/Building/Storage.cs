using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public Dictionary<RecourceType, float> Recources { get; private set; } = new Dictionary<RecourceType, float>();
    public event Action<RecourceType, float> RecourceChanged = delegate { };

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
        RecourceChanged.Invoke(type, Recources[type]);
    }

}
