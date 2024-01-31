using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDestroy : MonoBehaviour
{

    void Update()
    {
        if (transform.position.y < -20)
            Destroy(gameObject, 0);
    }
}
