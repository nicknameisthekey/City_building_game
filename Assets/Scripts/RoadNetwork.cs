using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork
{
    public static int NetworkNumbers = 0;
    // public Dictionary<RecourceType, float> Recources = new Dictionary<RecourceType, float>();
    public List<Building> buildingsInNetwork { get; private set; } = new List<Building>();
    //public Storage NetworkStorage = new Storage();
    public List<Storage> storages { get; private set; } = new List<Storage>();
    public int NetworkNum;
    public RoadNetwork()
    {
        NetworkNumbers++;
        NetworkNum = NetworkNumbers;
    }
    public void addBuildingToNetwork(Building building)
    {
        //простое добавление в список всех зданий в сети
        if (buildingsInNetwork.Contains(building))
        {
            Debug.Log("попытка добавить здание которое уже в списке");
        }
        else
        {
            buildingsInNetwork.Add(building);
        }
        if (building is StorageBuilding)
        {
            StorageBuilding storageBuilding = (StorageBuilding)building;
            storages.Add(storageBuilding.Storage);
            Debug.Log("распознал склад и добавил в сеть");
            //foreach (var rec in storageBuilding.Storage.Recources)
            //{
            //    NetworkStorage.Recources[rec.Key] += rec.Value;
            //}
        }
    }
    public bool ChangeRecourceInNetwork(RecourceType type, float amount)
    {
        foreach (var storage in storages)
        {
            if (storage.AddRecource(type, amount))
                return true;
        }
        return false;
        //Debug.Log("изменил ресуср в сети "+NetworkNum);
    }

    public static void MergeNetworks(RoadNetwork one, RoadNetwork two)
    {
        RoadNetwork newNetwork = consolidateNetworks(one, two);
        foreach (var building in newNetwork.buildingsInNetwork)
        {
            if (building is INetwork)
            {
                INetwork network = (INetwork)building;
                network.ChangeRoadNetwork(newNetwork);
            }
            else
                Debug.Log("в сети здание без интерфейса");
        }
    }
    private static RoadNetwork consolidateNetworks(RoadNetwork one, RoadNetwork two)
    {
        RoadNetwork newNetwork = new RoadNetwork();
        Debug.Log("consolidating, new network num = " + newNetwork.NetworkNum);
        //foreach (var resType in one.Recources.Keys)
        //{
        //    Debug.Log(resType.ToString());
        //    if (two.Recources.ContainsKey(resType))
        //    {
        //        Debug.Log("сложил ресурсы " + resType + " " + one.Recources[resType] + two.Recources[resType]);
        //        newStorage.Recources.Add(resType, one.Recources[resType] + two.Recources[resType]);
        //        two.Recources.Remove(resType);
        //    }
        //    else
        //    {
        //        Debug.Log("добавил ресурсы из первого" + resType + " " + one.Recources[resType]);
        //        newStorage.Recources.Add(resType, one.Recources[resType]);

        //    }
        //}
        //foreach (var resType in two.Recources.Keys)
        //{
        //    Debug.Log("добавил ресурсы из второго" + resType + " " + two.Recources[resType]);
        //    newStorage.Recources.Add(resType, two.Recources[resType]);

        //}
        foreach (var building in one.buildingsInNetwork)
        {
            if (two.buildingsInNetwork.Contains(building))
                Debug.Log("вторая сеть содержит здание которого там не должно быть!");
            newNetwork.addBuildingToNetwork(building);
        }
        foreach (var building in two.buildingsInNetwork)
        {
            newNetwork.addBuildingToNetwork(building);
        }
        return newNetwork;
    }

}
