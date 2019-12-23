using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public Dictionary<RecourceType, int> Recources { get; private set; } = new Dictionary<RecourceType, int>();
    public event Action<RecourceType, int, Storage> RecourceChanged = delegate { };
    public event Action<Storage> RecourcesChanged = delegate { };

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
        RecourcesChanged.Invoke(this);
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

    //новые
    public int GetFreeSpace()
    {
        return Capacity - TotalAmountOfGoods;
    }
    public int GetAmount(RecourceType type)
    {
        if (Recources.ContainsKey(type))
            return Recources[type];
        else return 0;
    }
    public bool SubstractMaximumAmount(RecourceType type, int amount, out int changed)
    {
        //убирает из склада максимальное количество
        //возвращает тру только если списано все количество ресурса
        //amount проходит положительный!
        if (amount <= 0)
            Debug.Log("прилетел amount <= 0, проверяй");

        int avaliable = GetAmount(type);

        if (avaliable == 0)
        {
            changed = 0;
            return false;
        }
        if (avaliable >= amount)
        {
            changeRecource(type, -amount);
            changed = amount;
            return true;
        }
        else
        {
            changeRecource(type, -avaliable);
            changed = avaliable;
            return false;
        }
    }
    public bool AddMaximumAmount(RecourceType type, int amount, out int changed)
    {
        int freeSpace = GetFreeSpace();
        Debug.Log("free " + freeSpace + " amount " + amount);
        if (AcceptableTypes.Contains(RecourceType.all) || AcceptableTypes.Contains(type))
        {
            if (freeSpace >= amount)
            {
                Debug.Log("f>=a, changed for a");
                changeRecource(type, amount);
                changed = amount;
                return true;
            }
            else
            {
                Debug.Log("f<a, changed for f");
                changeRecource(type, freeSpace);
                changed = freeSpace;
                return false;
            }
        }
        Debug.Log("unacceptable type");
        changed = 0;
        return false;
    }

    void changeRecource(RecourceType type, int amount)
    {
        if (amount == 0) return;

        if (Recources.ContainsKey(type))
            Recources[type] += amount;
        else
            Recources.Add(type, amount);
        TotalAmountOfGoods += amount;
        RecourceChanged.Invoke(type, Recources[type], this);
        RecourcesChanged.Invoke(this);
    }

}
