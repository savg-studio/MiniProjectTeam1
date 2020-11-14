using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // public params
    public float absorptionForce;
    public ForceMode2D forceMode;
    public float maxSize;
    public float growTime;
    public float centerSpawnTime;
    private float timePassed;

    private Vector2 baseScale;
    private BlackHoleCenter center;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        center = GetComponentInChildren<BlackHoleCenter>(true);

        Invoke("EnableCenter", centerSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        var scaler = timePassed / growTime;
        Vector3 finalScale = baseScale * scaler * maxSize;
        finalScale.z = 1;
        transform.localScale = finalScale;
    }

    void EnableCenter()
    {
        center.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var rigidBody = collision.attachedRigidbody;

        var spaceShip = collision.gameObject.GetComponent<Spaceship>();
        if (spaceShip)
            spaceShip.Stun(Mathf.Infinity);

        Absorb(rigidBody);
    }

    private void Absorb(Rigidbody2D rigidbody)
    {
        var tarPos = rigidbody.gameObject.transform.position;
        var center = transform.position;
        var dirVector = (center - tarPos).normalized;

        rigidbody.AddForce(dirVector * absorptionForce, forceMode);
    }
}
