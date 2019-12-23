using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Work
{
    AB_State_ProductionCycle state;
    ActiveBuildingParams abParams;
    public event Action ProgressChanged = delegate { }; 
    public AB_Work(AB_State_ProductionCycle state)
    {
        this.state = state; abParams = state.building.AbParams;
        ticksNeed = abParams.TicksToProduce;
    }
    public int ticksDone { get; private set; } = 0;
    public int ticksNeed { get; private set; }
    public void Initialization()
    {
        if (state.building.AbParams.InputRequired)
        {

        }
        else
        {
            startTicking();
        }
    }
    void startTicking()
    {
        if (canPlaceMoreOutput())
        {
            state.OutputSubstracted -= startTicking;
            Timer.OneSecond += onTick;
        }
    }
    void onTick()
    {
        ticksDone++;
        ProgressChanged.Invoke();
        Debug.Log(ticksDone);
        if (ticksDone >= ticksNeed)
        {
            ticksDone = 0;
            createOutput();
        }
    }

    void createOutput()
    {
        state.addOutput();
        if (!canPlaceMoreOutput())
        {
            Timer.OneSecond -= onTick;
            state.OutputSubstracted += startTicking;
            Debug.Log("больше чем капасити");
        }
    }
    bool canPlaceMoreOutput()
    {
        foreach (var kvp in state.OutputRecourcesLocal)
        {
            if (state.OutputRecourcesLocal[kvp.Key] + abParams.OutputRecources[kvp.Key] > abParams.OutputRecourceCapacity[kvp.Key])
                //новый output превысит capacity
                return false;
        }
        return true;
    }
}
