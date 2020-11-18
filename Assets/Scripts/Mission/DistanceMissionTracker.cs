using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMissionTracker : MissionTracker
{
    public float distanceToWin;
    public GameObject player;
    public GameObject origin;

    // Start is called before the first frame update
    void Start()
    {
        statusDisplay.SetAmount(0, distanceToWin);
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            Vector2 playerPos = player.transform.position;
            Vector2 originPos = origin.transform.position;

            float distance = (playerPos - originPos).magnitude;
            statusDisplay.SetAmount(distance, distanceToWin);

            if (isMaxDistanceReached(distance))
                Win();
        }
    }


    private bool isMaxDistanceReached(float distance)
    {
        return distance >= distanceToWin;
    }
}
