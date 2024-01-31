using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotationXY : MonoBehaviour
{
    int speedY;
    int speedX;
    // Start is called before the first frame update
    void Start()
    {
        speedX = Random.Range(40, 120);
        speedY = Random.Range(50, 120);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speedX*Time.deltaTime, speedY*Time.deltaTime, 0.0f, Space.Self);
    }
}
