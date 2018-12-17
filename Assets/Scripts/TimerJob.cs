using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;
using System.Threading;
using System.Collections;
using System;

public struct TimerJob : IJob
{
    public double startTime;
    public double maxTime;

    public double StartTime
    {
        get
        {
            return startTime;
        }

        set
        {
            startTime = value;
        }
    }

    public double MaxTime
    {
        get
        {
            return maxTime;
        }

        set
        {
            maxTime = value;
        }
    }

    public void Execute()
    {   
        while (StartTime <= MaxTime)
        {
            StartTime += 0.000001 * 0.08;
            LevelManager.TimeRemaninig = StartTime;
        }
    }
    
}
