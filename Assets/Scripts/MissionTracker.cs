using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    public float distanceToWin;
    public DistanceDisplay distanceDisplay;
    public MissionResultBanner resultBanner;
    public GameObject player;
    public GameObject origin;

    // Start is called before the first frame update
    void Start()
    {
        distanceDisplay.SetDistanceText(0, distanceToWin);
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            Vector2 playerPos = player.transform.position;
            Vector2 originPos = origin.transform.position;

            float distance = (playerPos - originPos).magnitude;
            distanceDisplay.SetDistanceText(distance, distanceToWin);

            if (isMaxDistanceReached(distance))
                Win();
        }
    }

    private void Win()
    {
        resultBanner.OnWin();
    }

    private bool isMaxDistanceReached(float distance)
    {
        return distance >= distanceToWin;
    }
}
