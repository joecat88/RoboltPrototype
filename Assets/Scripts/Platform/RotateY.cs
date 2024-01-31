using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY: Rotation
{

    void Update()
    {
        transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f, Space.Self);
    }

    //void Update()
    //{
    //    transform.Rotate(0.0f, 0.0f, speed, Space.Self);
    //}


}
