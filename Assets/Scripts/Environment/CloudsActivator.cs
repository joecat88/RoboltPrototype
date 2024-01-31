using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsActivator : MonoBehaviour
{
    public GameObject clouds;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            clouds.SetActive(true);
        }
    }
}
