using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeedScript
{
    public bool isOnOff;//false 시간, true 온오프 
    public float time;
    public float speed;

    public SpeedScript(float time, float speed)
    {
        if (time == 0f) isOnOff = true;
        else
        {
            this.time = time;
            isOnOff = false;
        }
        this.speed = speed;
    }
}
