using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;
using System.Threading;
using System.Collections;
using System;

public struct TimerJob : IJob
{
    public double StartTime;
    public double MaxTime;

        
    public void Execute()
    {   
        while (StartTime >= 0f)
        {
            LevelManager.TimeRemaninig = StartTime;
            
            StartTime+=0.000001*0.02;
        }
    }
  
}
