using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageOutput")
        {
            Destroy(other.gameObject.transform.parent.gameObject, 0);
        }
    }
}
