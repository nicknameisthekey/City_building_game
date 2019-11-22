using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork
{
    public Dictionary<RecourceType, float> Recources = new Dictionary<RecourceType, float>();
    public List<Building> buildingsInNetwork { get; private set; } = new List<Building>();
    public event Action<Dictionary<RecourceType, float>> RecourcesChanged = delegate { };
    private static RoadNetwork consolidateStorages(RoadNetwork one, RoadNetwork two)
    {
        RoadNetwork newStorage = new RoadNetwork();
        Debug.Log("wtf");
        foreach (var resType in one.Recources.Keys)
        {
            Debug.Log(resType.ToString());
            if (two.Recources.ContainsKey(resType))
            {
                Debug.Log("сложил ресурсы " + resType + " " + one.Recources[resType] + two.Recources[resType]);
                newStorage.Recources.Add(resType, one.Recources[resType] + two.Recources[resType]);
                two.Recources.Remove(resType);
            }
            else
            {
                Debug.Log("добавил ресурсы из первого" + resType + " " + one.Recources[resType]);
                newStorage.Recources.Add(resType, one.Recources[resType]);

            }

        }
        foreach (var resType in two.Recources.Keys)
        {
            Debug.Log("добавил ресурсы из второго" + resType + " " + two.Recources[resType]);
            newStorage.Recources.Add(resType, two.Recources[resType]);

        }
        return newStorage;
    }
    public static void MergeStorages(RoadNetwork one, RoadNetwork two)
    {
        RoadNetwork newStorage = consolidateStorages(one, two);
        foreach (var road in one.buildingsInNetwork)
        {
            Debug.Log("добавил в новый список дорогу из первой сети");
            newStorage.addBuildingToNetwork(road);
            road.ChangeStorage(newStorage);
        }
        foreach (var road in two.buildingsInNetwork)
        {
            Debug.Log("добавил в новый список дорогу из второй сети");
            newStorage.addBuildingToNetwork(road);
            road.ChangeStorage(newStorage);
        }
    }
    public void changeRecourceAmount(RecourceType type, float amount)
    {
        if (Recources.ContainsKey(type))
        {
            Recources[type] += amount;
        }
        else
        {
            Recources.Add(type, amount);
        }
        RecourcesChanged.Invoke(Recources);
    }


    public void addBuildingToNetwork(Building building)
    {
        if (buildingsInNetwork.Contains(building))
        {
            Debug.Log("попытка добавить здание которое уже в списке");
        }
        else
        {
            buildingsInNetwork.Add(building);
        }
    }
}
