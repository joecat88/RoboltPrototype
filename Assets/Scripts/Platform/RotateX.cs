using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateX: Rotation
{

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, speed * Time.deltaTime, Space.Self);
    }
    
}
