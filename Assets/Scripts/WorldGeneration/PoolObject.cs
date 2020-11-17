using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PoolGameObject
{
    GameObject GetGameObject();

    void InitGameObject();

    void ResetGameObject();
}
