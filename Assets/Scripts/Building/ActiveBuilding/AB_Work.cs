using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Work
{
    AB_State_ProductionCycle state;
    AB_Input input;
    AB_Output output;
    ActiveBuildingParams abParams;
    bool workPermited = true;
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
        output = state.Ab_Output;
        input = state.AB_Input;
        startTicking();
    }
    public void StartStop()
    {
        workPermited = !workPermited;
    }
    void startTicking()
    {
        input.ProductionAvaliable -= startTicking;
        output.ProductionAvaliable -= startTicking;
        if (output.ProductionFlag && input.ProductionFlag)
        {
            Debug.Log(state.building.BuildingName + " начало произвоства");
            state.substractInput();
            Timer.OneSecond += onTick;
        }
        else
        {
            input.ProductionAvaliable += startTicking;
            output.ProductionAvaliable += startTicking;
            Debug.Log(state.building.BuildingName + " input " + input.ProductionFlag + " output " + output.ProductionFlag);
        }
    }
    void onTick()
    {
        if (workPermited)
        {
            ticksDone++;
            ProgressChanged.Invoke();
           // Debug.Log(ticksDone);
            if (ticksDone >= ticksNeed)
            {
                ticksDone = 0;
                Timer.OneSecond -= onTick;
                output.AddOutput();
                startTicking();
            }
        }
    }
}
