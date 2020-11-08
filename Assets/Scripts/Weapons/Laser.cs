using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Spaceship owner;

    public void SetOwner(Spaceship owner)
    {
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spaceship ss = collision.gameObject.GetComponent<Spaceship>();

        if (ss && owner != ss)
            ss.TakeDamage();
    }
}
