using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIssionObjective : MonoBehaviour
{
    public int amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var potentialPlayer = collision.GetComponent<Player>();
        if (potentialPlayer)
        {
            potentialPlayer.UpdateMission(amount);
            gameObject.SetActive(false);
        }
    }
}
