using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTrapRotationXY : MonoBehaviour
{
    public int speedY;
    public int speedX;

    void Update()
    {
        transform.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0.0f, Space.Self);
    }
}
