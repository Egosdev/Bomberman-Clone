using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSpawn : MonoBehaviour
{
    // destroy bricks
    public float destroyAfterSec = 1f;
    void Start()
    {
        Destroy(gameObject, destroyAfterSec);
    }
}
