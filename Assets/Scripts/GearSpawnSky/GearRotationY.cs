using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotationY : MonoBehaviour
{
    int speedY;
    // Start is called before the first frame update
    void Start()
    {
        speedY = Random.Range(50, 110);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, speedY * Time.deltaTime, 0.0f, Space.Self);
    }
}
