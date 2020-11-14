using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    // Public params
    public float duration;

    // Inner
    private bool isStopped;
    private float timeLeft;

    Action callback;

    public Timer(float duration)
    {
        this.duration = duration;
    }

    public void Start()
    {
        timeLeft = duration;
    }

    public void Stop()
    {
        isStopped = true;
    }

    public void Resume()
    {
        isStopped = false;
    }

    public void Restart()
    {
        Start();
        isStopped = false;
    }

    public void Update()
    {
        if (!isStopped)
        {
            timeLeft -= Time.deltaTime;
            if (IsOver())
            {
                callback();
                isStopped = true;
            }
        }
    }

    public bool IsOver()
    {
        return timeLeft <= 0;
    }

    public void SetCallback(Action callback)
    {
        this.callback = callback;
    }
}
