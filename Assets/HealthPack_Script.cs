using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Nave = collision.gameObject;
        Player player = Nave.GetComponent<Player>();

        if (player)
        {
            player.vida += 50;
            if (player.vida > player.vida_max)
            {
                player.vida = player.vida_max;
            }
            this.gameObject.SetActive(false);
        }

    }
}
