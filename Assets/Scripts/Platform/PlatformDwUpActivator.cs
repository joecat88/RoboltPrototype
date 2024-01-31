using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDwUpActivator : MonoBehaviour
{
    public PlatformMovingDwUp platform;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            platform.SetIsStopped(false);
            Destroy(this, 0);
        }
    }
}
