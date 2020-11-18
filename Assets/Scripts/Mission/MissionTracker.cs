using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    public MissionResultBanner resultBanner;
    public MissionStatusDisplay statusDisplay;

    public void Win()
    {
        resultBanner.OnWin();
    }

    public void Fail()
    {
        resultBanner.OnFail();
    }

    public virtual void UpdateMissionProgress(int amount)
    {

    }
}
