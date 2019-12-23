using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AB_Output
{
    AB_State_ProductionCycle state;
    ActiveBuildingParams abParams;
    public AB_Output(AB_State_ProductionCycle state)
    {
        this.state = state;
        abParams = state.building.AbParams;
    }
    public void Initialization()
    {
        state.OutputAdded += onOutputAdd;
        //foreach (var st in state.building.ReachableStorages)
        //{
        //    st.Key.Storage.RecourcesChanged += onStorageRecourcesChanged;
        //}
    }
    void onOutputAdd()
    {
        tryAllStoragesGiveOutput();
    }
    void onStorageRecourcesChanged(Storage storage)
    {
        tryStorageGiveOutput(storage);
    }
    public void OnNewStorageAvaliable(Storage storage)
    {
        storage.RecourcesChanged += onStorageRecourcesChanged;
        tryStorageGiveOutput(storage);
    }
    void tryAllStoragesGiveOutput()
    {
        foreach (var st in state.building.ReachableStorages)
        {
            tryStorageGiveOutput(st.Key.Storage);
            if (checkOutputLocalEmpty())
                break;
        }
    }
    void tryStorageGiveOutput(Storage storage)
    {
       // Debug.Log("внутри");
        var kvps = state.OutputRecourcesLocal.ToList();
        foreach (var res in kvps)
        {
            if (res.Value == 0) break;
            storage.RecourcesChanged -= onStorageRecourcesChanged;
            storage.AddMaximumAmount(res.Key, res.Value, out int changed);
           // Debug.Log("changed "+ changed);
            if (changed != 0)
            {
               // Debug.Log("<color=red>"+res.Value + " changed " + changed+"</color>");
                state.SubstractOutput(res.Key, changed);
            }
            storage.RecourcesChanged += onStorageRecourcesChanged;

        }
      //  Debug.Log("вышел");
    }
    bool checkOutputLocalEmpty()
    {
        foreach (var res in state.OutputRecourcesLocal)
        {
            if (res.Key != 0)
                return false;
        }
        return true;
    }
}
