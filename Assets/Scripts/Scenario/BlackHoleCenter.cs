using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCenter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            var potentialPlayer = collision.gameObject.GetComponent<Player>();
            if (potentialPlayer)
                potentialPlayer.Die();
            else
            {
                //Debug.Log(collision.gameObject.name + " was destroyed by black hole");
                collision.gameObject.SetActive(false);
            }
        }
    }
}
