using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMissionTracker : MissionTracker
{
    public int objectiveResources;
    private int currentResources;

    private void Start()
    {
        currentResources = 0;
        statusDisplay.SetAmountInt(0, objectiveResources);
    }

    public override void UpdateMissionProgress(int amount)
    {
        currentResources += amount;
        if (currentResources >= objectiveResources)
            Win();

        statusDisplay.SetAmountInt(currentResources, objectiveResources);
    }
}
