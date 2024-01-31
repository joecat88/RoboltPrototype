using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
            Debug.Log("Collisionecfergfver");
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log("Distruggi");
            Destroy(gameObject, 0);
        }
    }
}
