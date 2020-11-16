using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack_Script : MonoBehaviour
{
    public uint armorRestored;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject Nave = collision.gameObject;
        Player player = Nave.GetComponent<Player>();

        if (player)
        {
            player.RestoreArmor(armorRestored);
            this.gameObject.SetActive(false);
        }
    }
}
