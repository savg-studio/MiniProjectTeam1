using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Public params
    public float shieldCooldown;

    // Private
    private bool isActive;
    private Timer timer;

    // Cache shield object
    GameObject innerObject;

    // Start is called before the first frame update
    void Start()
    {
        // Prepare timer
        timer = new Timer();
        timer.duration = shieldCooldown;
        timer.SetCallback(Enable);

        innerObject = transform.GetChild(0).gameObject;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActive())
        {
            timer.Update();
        }  
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void Enable()
    {
        isActive = true;
        innerObject.SetActive(true);
    }

    public void Disable()
    {
        isActive = false;
        innerObject.SetActive(false);
        timer.Start();
    }
}
