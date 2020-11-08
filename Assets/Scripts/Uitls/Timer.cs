using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    // Public params
    public float duration;

    // Inner
    private float timeLeft;

    Action callback;

    public void Start()
    {
        timeLeft = duration;
    }

    public void Update()
    {
        timeLeft -= Time.deltaTime;
        if (IsOver())
            callback();
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
