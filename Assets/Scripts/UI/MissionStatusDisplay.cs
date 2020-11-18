using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionStatusDisplay : MonoBehaviour
{
    public string stringPrefix;

    // Cache
    private Text distanceDisplay;

    public void Awake()
    {
        distanceDisplay = GetComponent<Text>();
        SetAmountInt(0, 0);
    }

    public void SetAmount(float minAmount, float maxAmount)
    {
        var text = stringPrefix + minAmount.ToString("F2") + "/" + maxAmount.ToString("F2");
        
        distanceDisplay.text = text;
    }

    public void SetAmountInt(int minAmount, float maxAmount)
    {
        var text = stringPrefix + minAmount + "/" + maxAmount;

        distanceDisplay.text = text;
    }
}
