using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Script : MonoBehaviour
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
        GameObject Nave= collision.gameObject;
        Player player = Nave.GetComponent<Player>();

        if (player)
        {
            player.vida -= 10;
            this.gameObject.SetActive(false);
        }
        
    }
}
