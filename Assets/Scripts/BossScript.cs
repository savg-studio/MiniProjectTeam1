using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    LEFT,
    RIGHT
}

public class BossScript : MonoBehaviour
{
    public GameObject faceRef;
    public GameObject bossAttackZone;
    public List<GameObject> bossZones;
    public GameObject bulletPrefab;
    public float speed;
    public float waypointThreshold;
    public float waitTime;
    public float minShootInterval;
    public float maxShootInterval;
    public float shootSpeed;

    // Cache
    private Rigidbody2D rigidbody;

    // State
    private Vector2 currentPosTarget;
    private int currentZoneId;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        // Cache
        rigidbody = GetComponent<Rigidbody2D>();

        // Init
        currentZoneId = -1;
        moving = true;
        SetNextTarget();

        Invoke("Shoot", GetNextShotTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (HasReachedTarget())
            {
                SetNextTarget();
                StopMoving();
                Invoke("StartMoving", waitTime);
            }
            else
            {
                MoveToTarget();
            }
        }
    }

    private void Shoot()
    {

        // Create bullet
        var pos = faceRef.transform.position;
        GameObject go = GameObject.Instantiate(bulletPrefab, pos, Quaternion.identity);

        // Shoot bullet
        var bullet = go.GetComponent<Bullet>();
        bullet.Init();
        bullet.Launch(GetShotDir(), shootSpeed);


        Invoke("Shoot", GetNextShotTime());
    }

    private float GetNextShotTime()
    {
        return Random.Range(minShootInterval, maxShootInterval);
    }

    private Vector2 GetShotDir()
    {
        Vector2 refPos = faceRef.transform.position;
        Vector2 dir = (refPos - GetCurrentPos()).normalized;
        return dir;
    }

    private void SetNextTarget()
    {
        currentZoneId = (currentZoneId + 1) % bossZones.Count; 
        currentPosTarget = GetNextPosTarget(currentZoneId);
    }

    private bool HasReachedTarget()
    {
        return GetDistanceToTarget() <= waypointThreshold;
    }
    private void MoveToTarget()
    {
        var dir = GetDirToTarget();
        var pos = GetCurrentPos();
        rigidbody.MovePosition(pos + dir * speed);
    }

    private Vector2 GetDirToTarget()
    {
        return (currentPosTarget - GetCurrentPos()).normalized;
    }

    private float GetDistanceToTarget()
    {
        var pos = GetCurrentPos();
        var distance = (currentPosTarget - pos).magnitude;

        return distance;
    }

    private Vector2 GetCurrentPos()
    {
        return transform.position;
    }

    private Vector2 GetNextPosTarget(int zoneId)
    {
        var zone = bossZones[zoneId].GetComponent<BoxCollider2D>();
        var bounds = zone.bounds;
        return T1Utils.GetRandomPointInBounds(bounds);
    }

    private void StartMoving()
    {
        moving = true;
    }

    private void StopMoving()
    {
        moving = false;
    }
}
