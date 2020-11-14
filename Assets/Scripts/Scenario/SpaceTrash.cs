using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrash : MonoBehaviour
{

    private float torque;
    private float dir;

    private Rigidbody2D rigidbody;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        dir = Random.Range(0, 2) == 1 ? 1 : -1;
    }

    public void SetTorque(float minTorque, float maxTorque)
    {
        torque = Random.Range(minTorque, maxTorque) * dir;
    }

    public void SetRandomRotation()
    {
        float rotation = Random.Range(-180, 180);
        //rigidbody.SetRotation(rotation);

        var baseRot = transform.localRotation.eulerAngles;
        var quat = Quaternion.Euler(baseRot.x, baseRot.y, rotation);
        transform.localRotation = quat;
    }

    private void OnEnable()
    {
        rigidbody.angularVelocity = torque;
    }
}
