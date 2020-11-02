using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        GameObject.Destroy(this);
    }

    public override void OnCollide(Collision2D collision)
    {
        GameObject.Destroy(this.gameObject);
    }
}
