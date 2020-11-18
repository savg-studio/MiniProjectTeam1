using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : MonoBehaviour
{
    public GameObject go;
    public int size;

    // Inner
    private List<PoolGameObject> gameObjects;
    private int index;
    private bool wrapped;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;

        gameObjects = new List<PoolGameObject>();
        // Initialize pool
        for (int i = 0; i < size; i++)
        {
            var clone = GameObject.Instantiate(go);
            clone.SetActive(false);
            clone.transform.parent = transform;
            var poolObject = clone.GetComponent<PoolGameObject>();

            Assert.IsNotNull(poolObject, "GameObject was not a poolObject");

            poolObject.InitGameObject();
            gameObjects.Add(poolObject);
        }
    }
        
    public PoolGameObject GetGameObject()
    {
        // Get 
        var go = gameObjects[index];

        // Reset it if wrapped
        if (wrapped)
            go.ResetGameObject();

        // Increase index
        index = (index + 1) % size;

        // Check if wrapped around the object pool
        if (index == 0)
        {
            wrapped = true;
            Debug.Log(gameObject.name + " wrapped around");
        }

        return go;
    }

    public PoolGameObject GetGameObject(Vector2 pos)
    {
        var go = GetGameObject();
        go.GetGameObject().transform.position = pos;

        return go;
    }

    public PoolGameObject GetGameObject(Vector2 pos, Transform parent)
    {
        var go = GetGameObject(pos);
        go.GetGameObject().transform.parent = parent;

        return go;
    }
}
