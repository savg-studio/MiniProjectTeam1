using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour
{
    public string stringPrefix;

    // Cache
    private Text distanceDisplay;

    public void Awake()
    {
        distanceDisplay = GetComponent<Text>();
        SetDistanceText(0, 500);
    }

    public void SetDistanceText(float distance, float maxDistance)
    {
        var text = stringPrefix + distance.ToString("F2") + "/" + maxDistance.ToString("F2");
        
        distanceDisplay.text = text;
    }
}
