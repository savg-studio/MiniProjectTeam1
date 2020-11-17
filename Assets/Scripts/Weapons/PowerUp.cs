using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, PoolGameObject
{
    public GameObject weaponGO;

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void InitGameObject()
    {
        
    }

    public void ResetGameObject()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            var weaponClone = GameObject.Instantiate(weaponGO, player.transform);
            var weapon = weaponClone.GetComponent<WeaponBase>();
            player.SetSecondWeapon(weapon);

            this.gameObject.SetActive(false);
        }
    }
}
