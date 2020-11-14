using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorZone : MonoBehaviour
{
    private BoxCollider2D collider;

    public uint maxMeteors;

    private List<Meteor> meteors = new List<Meteor>();
    // Start is called before the first frame update
    void Awake()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var meteor = collision.gameObject.GetComponent<Meteor>();

        if(meteor)
        {
            meteors.Remove(meteor);
            Destroy(meteor.gameObject);
        }

    }
    public void AddMeteor(Meteor ms)
    {
        meteors.Add(ms);
    }

    public bool IsFull()
    {
        return meteors.Count >= maxMeteors;
    }

    public Vector2 GetRandomPointInBounds()
    {
        var bounds = collider.bounds;
        return T1Utils.GetRandomPointInBounds(bounds);
    }
}
